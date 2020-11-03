import React, { useEffect, useState } from "react";
import Axios from "axios";
import { history } from "../redux/history";
import moment from "moment";
import ValidInput from "./ValidInput";

const Worker = (ownProps) => {
  const [worker, setWorker] = useState(null);
  const [defaultWorker, setDefaultWorker] = useState(null);
  const [areChanges, setChanges] = useState(false);

  const [isValid, setValid] = useState(true);
  const [invalidFields, setInvalidFields] = useState(0);

  const [statuses, setStatuses] = useState([]);
  const [genders, setGenders] = useState([]);
  const [garages, setGarages] = useState([]);

  useEffect(() => {
    Axios.post("http://localhost:5000/api/sql", {
      query: `SELECT * FROM worker_status`,
    }).then((res1) => {
      setStatuses([...res1.data]);

      Axios.post("http://localhost:5000/api/sql", {
        query: `SELECT * FROM gender`,
      }).then((res2) => {
        setGenders([...res2.data]);

        Axios.post("http://localhost:5000/api/sql", {
          query: `SELECT * FROM garage`,
        }).then((res3) => {
          setGarages([...res3.data]);

          let status = 1;
          if ([...res1.data].length > 0) status = [...res1.data][0].id;
          let gender = 1;
          if ([...res2.data].length > 0) gender = [...res2.data][0].id;
          let garage = 1;
          if ([...res3.data].length > 0) garage = [...res3.data][0].id;

          if (ownProps.match.params.id === "new") {
            const work = {
              id: 0,
              name: "",
              surname: "",
              phone_number: "",
              email: "",
              birth_date: moment().format("YYYY-MM-DD"),
              worker_status: status,
              gender: gender,
              fk_GARAGE: garage,
            };

            setWorker(work);
            setDefaultWorker({ ...work });
            return;
          }

          Axios.post("http://localhost:5000/api/sql", {
            query: `SELECT * FROM worker 
            WHERE worker.id=${ownProps.match.params.id}`,
          }).then((res4) => {
            setWorker(res4.data[0]);
            setDefaultWorker(res4.data[0]);
            console.log(res4.data[0]);
          });
        });
      });
    });
  }, [ownProps.match.params.id]);

  useEffect(() => {
    if (JSON.stringify(worker) !== JSON.stringify(defaultWorker))
      setChanges(true);
    else setChanges(false);
  }, [worker]);

  const handleValidation = (valid) => {
    if (valid) {
      setValid(invalidFields - 1 === 0);
      setInvalidFields(invalidFields - 1);
    } else {
      setValid(false);
      setInvalidFields(invalidFields + 1);
    }
  };

  return worker === null ? (
    <div></div>
  ) : (
    <div className="container border p-3 mt-2">
      <div className="container border">
        <div
          style={{
            width: "fit-content",
            position: "relative",
            top: -14,
            backgroundColor: "#FFF",
          }}
          className="m-0 px-2"
        >
          Worker information
        </div>
        <table className="table table-borderless table-sm">
          <tbody>
            <tr>
              <td className="text-right">ID:</td>
              <td>{worker.id === "new" ? "-" : worker.id}</td>
            </tr>
            <tr>
              <td className="text-right">name:</td>
              <td>
                <ValidInput
                  type="text"
                  value={worker.name}
                  onChange={(e) => {
                    setWorker({
                      ...worker,
                      name: e.target.value,
                    });
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">suname:</td>
              <td>
                <ValidInput
                  type="text"
                  value={worker.surname}
                  onChange={(e) => {
                    setWorker({
                      ...worker,
                      surname: e.target.value,
                    });
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">phone number:</td>
              <td>
                <ValidInput
                  type="text"
                  value={worker.phone_number}
                  onChange={(e) => {
                    setWorker({
                      ...worker,
                      phone_number: e.target.value,
                    });
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">email:</td>
              <td>
                <ValidInput
                  type="text"
                  value={worker.email}
                  onChange={(e) => {
                    setWorker({
                      ...worker,
                      email: e.target.value,
                    });
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">Birth date:</td>
              <td>
                <ValidInput
                  type="date"
                  onChange={(e) => {
                    setWorker({ ...worker, birth_date: e.target.value });
                  }}
                  value={worker.birth_date.substring(0, 10)}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">status:</td>
              <td>
                <select
                  style={{ height: 30 }}
                  onChange={(e) => {
                    setWorker({
                      ...worker,
                      worker_status: Number(e.target.value),
                    });
                  }}
                  value={worker.worker_status}
                >
                  {statuses.map((status) => {
                    return (
                      <option
                        key={status.id}
                        value={status.id}
                      >{`${status.name}`}</option>
                    );
                  })}
                </select>
              </td>
            </tr>
            <tr>
              <td className="text-right">gender:</td>
              <td>
                <select
                  style={{ height: 30 }}
                  onChange={(e) => {
                    setWorker({
                      ...worker,
                      gender: Number(e.target.value),
                    });
                  }}
                  value={worker.gender}
                >
                  {genders.map((gender) => {
                    return (
                      <option
                        key={gender.id}
                        value={gender.id}
                      >{`${gender.name}`}</option>
                    );
                  })}
                </select>
              </td>
            </tr>
            <tr>
              <td className="text-right">workplace:</td>
              <td>
                <select
                  style={{ height: 30 }}
                  onChange={(e) => {
                    setWorker({
                      ...worker,
                      fk_GARAGE: Number(e.target.value),
                    });
                  }}
                  value={worker.fk_GARAGE}
                >
                  {garages.map((garage) => {
                    return (
                      <option
                        key={garage.id}
                        value={garage.id}
                      >{`${garage.name}`}</option>
                    );
                  })}
                </select>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div className="row justify-content-center mt-3">
        <button
          className="btn btn-primary"
          disabled={
            !areChanges ||
            !isValid ||
            statuses.length === 0 ||
            genders.length === 0 ||
            garages.length === 0
          }
          onClick={() => {
            Axios.post("http://localhost:5000/api/sql", {
              query: `INSERT INTO worker(id, name, surname, phone_number, email, 
                                        birth_date, worker_status, gender, fk_GARAGE) 
                      VALUES (${worker.id}, '${worker.name}', '${worker.surname}',
                              '${worker.phone_number}', '${worker.email}', 
                              '${worker.birth_date}', '${worker.worker_status}', 
                              '${worker.gender}', '${worker.fk_GARAGE}')
                      ON DUPLICATE KEY UPDATE 
                      name='${worker.name}', 
                      surname='${worker.surname}', 
                      phone_number='${worker.phone_number}', 
                      email='${worker.email}', 
                      birth_date='${worker.birth_date}', 
                      worker_status='${worker.worker_status}', 
                      gender='${worker.gender}', 
                      fk_GARAGE='${worker.fk_GARAGE}'`,
            })
              .then((res) => {
                history.push("/workers");
              })
              .catch((e) => {});
          }}
        >
          save
        </button>
      </div>
    </div>
  );
};

export default Worker;
