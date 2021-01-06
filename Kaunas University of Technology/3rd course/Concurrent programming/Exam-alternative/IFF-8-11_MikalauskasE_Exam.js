const { start, dispatch, stop, spawnStateless, spawn } = require("nact");
const system = start();
const fs = require("fs");
const crypto = require("crypto");
const table = require("easy-table");
const dataFiles = [
  "IFF-8-11_MikalauskasE_Exam_dat_1.json",
  "IFF-8-11_MikalauskasE_Exam_dat_2.json",
  "IFF-8-11_MikalauskasE_Exam_dat_3.json",
];
const resFiles = [
  "IFF-8-11_MikalauskasE_Exam_res_1.txt",
  "IFF-8-11_MikalauskasE_Exam_res_2.txt",
  "IFF-8-11_MikalauskasE_Exam_res_3.txt",
];
const distributeType = {
  FROM_MAIN: "from_main",
  FROM_MAIN_COUNT: "from_main_count",
  FROM_WORKER: "from_worker",
  FROM_RESULTS: "from_results",
};

// returns a hash(SHA1) string of a student object (hashed in format "{name year grade} to match previous lab tasks")
const hashStudent = (student) => {
  return crypto
    .createHash("sha1")
    .update(`{${student.name} ${student.year} ${student.grade.toFixed(2)}}`)
    .digest("hex");
};

// returns the count of digits in a given string
const countDigits = (string) => {
  return string
    .split("")
    .reduce(
      (accumulator, currentValue) =>
        isNaN(currentValue) ? accumulator : accumulator + 1,
      0
    );
};

// work with all files
dataFiles.map((dataFile, index) => {
  const filedata = fs.readFileSync(dataFile);
  const students = JSON.parse(filedata);

  const workerCount = 5;

  // actor that distributes messages to other actors
  const distributor = spawn(
    system,
    (state = { count: 0, workerIndex: 0 }, msg) => {
      switch (msg.type) {
        case distributeType.FROM_MAIN: // when receiving student from main
          dispatch(workers[state.workerIndex], { payload: msg.payload });
          return {
            ...state,
            workerIndex: (state.workerIndex + 1) % workerCount,
          };

        case distributeType.FROM_MAIN_COUNT: // when receiving count of students from main
          dispatch(result, { maxCount: msg.payload });
          return state;

        case distributeType.FROM_WORKER: // when receiving filtered student from worker actor
          dispatch(result, { payload: msg.payload, count: state.count + 1 });
          return { ...state, count: state.count + 1 };

        case distributeType.FROM_RESULTS: // when receiving all results from result actor
          dispatch(printer, { payload: msg.payload });
          return state;

        default:
          return state;
      }
    },
    dataFile + "distributor"
  );

  // array of worker actors
  const workers = Array(workerCount)
    .fill(0)
    .map((_, i) => {
      return spawnStateless(
        distributor,
        (msg, ctx) => {
          const count = countDigits(hashStudent(msg.payload));
          if (count > 20)
            dispatch(distributor, {
              type: distributeType.FROM_WORKER,
              payload: { ...msg.payload, count: count },
            });
          else dispatch(distributor, { type: distributeType.FROM_WORKER });
        },
        dataFile + "worker" + i
      );
    });

  // actor that stores sorted results
  const result = spawn(
    distributor,
    (state = { results: [], maxCount: Infinity }, msg, ctx) => {
      if (msg.maxCount) state.maxCount = msg.maxCount;
      if (msg.payload) {
        // if received payload (student)
        // find index where to insert
        const to = state.results.findIndex(
          (el) => el.count < msg.payload.count
        );
        // if index not found add to end of array
        if (to === -1) state.results.push(msg.payload);
        // else insert to found index place
        else state.results.splice(to, 0, msg.payload);
      }
      if (msg.count === state.maxCount)
        dispatch(distributor, {
          type: distributeType.FROM_RESULTS,
          payload: state.results,
        });
      return state;
    },
    dataFile + "result"
  );

  // actor that saves all received results and initial data to a file
  const printer = spawnStateless(
    distributor,
    (msg, ctx) => {
      // initial data table
      const dataTable = new table();
      students.forEach((student, i) => {
        dataTable.cell("index", i + 1);
        dataTable.cell("name", student.name);
        dataTable.cell("year", student.year);
        dataTable.cell("grade", student.grade);
        dataTable.newRow();
      });
      // received results table
      const resTable = new table();
      msg.payload.forEach((student, i) => {
        resTable.cell("index", i + 1);
        resTable.cell("name", student.name);
        resTable.cell("year", student.year);
        resTable.cell("grade", student.grade);
        resTable.cell("count", student.count);
        resTable.newRow();
      });

      const str = `${dataFile}:\n${dataTable.toString()}\n\nresults:\n${resTable.toString()}`;

      fs.writeFile(resFiles[index], str, (err) => {
        if (err) throw err;
      });
    },
    dataFile + "printer"
  );

  // send count of students to distributor
  dispatch(distributor, {
    type: distributeType.FROM_MAIN_COUNT,
    payload: students.length,
  });
  // send students to distributor
  students.forEach((student) => {
    dispatch(distributor, {
      type: distributeType.FROM_MAIN,
      payload: student,
    });
  });
});
