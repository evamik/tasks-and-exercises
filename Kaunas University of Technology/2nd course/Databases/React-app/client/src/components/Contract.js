import React, { useEffect, useState } from "react";
import Axios from "axios";
import { history } from "../redux/history";
import moment from "moment";
import ValidInput from "./ValidInput";

const Contract = (ownProps) => {
  const [contract, setContract] = useState(null);
  const [defaultContr, setDefaultContr] = useState(null);
  const [areChanges, setChanges] = useState(false);

  const [workers, setWorkers] = useState([]);
  const [clients, setClients] = useState([]);

  const [bills, setBills] = useState([]);
  const [defaultBills, setDefaultBills] = useState([]);
  const [deletedBills, setDeletedBills] = useState([]);

  const [services, setServices] = useState([]);
  const [defaultServices, setDefaultServices] = useState([]);
  const [deletedServices, setDeletedServices] = useState([]);
  const [serviceList, setServiceList] = useState([]);

  const [parts, setParts] = useState([]);
  const [defaultParts, setDefaultParts] = useState([]);
  const [deletedParts, setDeletedParts] = useState([]);
  const [partList, setPartList] = useState([]);

  const [isValid, setValid] = useState(true);
  const [invalidFields, setInvalidFields] = useState(0);

  useEffect(() => {
    Axios.post("http://localhost:5000/api/sql", {
      query: `SELECT worker.id, worker.name, worker.surname
            FROM worker`,
    }).then((res2) => {
      setWorkers([...res2.data]);

      Axios.post("http://localhost:5000/api/sql", {
        query: `SELECT client.personal_code AS 'id', client.name, client.surname
            FROM client`,
      }).then((res3) => {
        setClients([...res3.data]);

        Axios.post("http://localhost:5000/api/sql", {
          query: `SELECT * FROM service`,
        }).then((res6) => {
          setServiceList([...res6.data]);

          Axios.post("http://localhost:5000/api/sql", {
            query: `SELECT * FROM parts`,
          }).then((res8) => {
            setPartList([...res8.data]);
            let worker = 1;
            if ([...res2.data].length > 0) worker = [...res2.data][0].id;
            let client = 0;
            if ([...res3.data].length > 0) client = [...res3.data][0].id;

            if (ownProps.match.params.id === "new") {
              const contr = {
                id: 0,
                order_date: moment().format("YYYY-MM-DD"),
                repair_start_date: moment().format("YYYY-MM-DD"),
                expected_end_date: moment().format("YYYY-MM-DD"),
                real_end_date: moment().format("YYYY-MM-DD"),
                fk_WORKER: worker,
                fk_CLIENT: client,
                sum: 0,
                additional_costs: 0,
              };

              let service = 1;
              if ([...res6.data].length > 0) service = [...res6.data][0].id;
              const ordered_service = {
                id: 0,
                count: 1,
                fk_SERVICE: service,
                fk_CONTRACT: 0,
              };
              setServices([ordered_service]);
              setContract(contr);
              setDefaultContr({ ...contr });
              return;
            }

            Axios.post("http://localhost:5000/api/sql", {
              query: `SELECT * FROM contract 
            WHERE contract.id=${ownProps.match.params.id}`,
            }).then((res) => {
              Axios.post("http://localhost:5000/api/sql", {
                query: `SELECT * FROM bill
            WHERE bill.fk_CONTRACT=${ownProps.match.params.id}`,
              }).then((res4) => {
                setBills([...res4.data]);
                setDefaultBills(JSON.parse(JSON.stringify([...res4.data])));

                Axios.post("http://localhost:5000/api/sql", {
                  query: `SELECT *
                      FROM ordered_service
                      WHERE ordered_service.fk_CONTRACT=${ownProps.match.params.id}`,
                }).then((res5) => {
                  setServices([...res5.data]);
                  setDefaultServices(
                    JSON.parse(JSON.stringify([...res5.data]))
                  );

                  Axios.post("http://localhost:5000/api/sql", {
                    query: `SELECT *
                      FROM parts_used
                      WHERE parts_used.fk_CONTRACT=${ownProps.match.params.id}`,
                  }).then((res7) => {
                    setParts([...res7.data]);
                    setDefaultParts(JSON.parse(JSON.stringify([...res7.data])));

                    setDefaultContr(res.data[0]);
                    setContract(res.data[0]);
                  });
                });
              });
            });
          });
        });
      });
    });
  }, [ownProps.match.params.id]);

  useEffect(() => {
    if (
      JSON.stringify(contract) !== JSON.stringify(defaultContr) ||
      JSON.stringify(bills) !== JSON.stringify(defaultBills) ||
      JSON.stringify(services) !== JSON.stringify(defaultServices) ||
      JSON.stringify(parts) !== JSON.stringify(defaultParts)
    )
      setChanges(true);
    else setChanges(false);
  }, [contract, bills, services, parts]);

  const handleValidation = (valid) => {
    if (valid) {
      setValid(invalidFields - 1 === 0);
      setInvalidFields(invalidFields - 1);
    } else {
      setValid(false);
      setInvalidFields(invalidFields + 1);
    }
  };

  const mapPeople = (people) => {
    return people.map((person) => {
      return (
        <option
          key={person.id}
          value={person.id}
        >{`${person.name} ${person.surname}`}</option>
      );
    });
  };

  const mapBills = (bills) => {
    return bills.map((bill, index) => {
      return (
        <tr key={index}>
          <td>
            <ValidInput
              type="date"
              value={bill.date.substring(0, 10)}
              onChange={(e) => {
                const arr = [...bills];
                arr[index].date = e.target.value;
                setBills(arr);
              }}
            />
          </td>
          <td>
            <ValidInput
              type="above zero number"
              value={bill.sum}
              onChange={(e) => {
                if (e.target.value < 0) return;
                const arr = [...bills];
                arr[index].sum = Number(e.target.value);
                setBills(arr);
              }}
              validation={handleValidation}
            />
          </td>
          <td>
            <button
              className="btn btn-sm btn-danger py-0 px-2 m-0 mr-3"
              onClick={() => {
                if (bill.number !== 0)
                  setDeletedBills([...deletedBills, bills[index]]);
                setBills(
                  bills.filter((b, id) => {
                    return id !== index;
                  })
                );
              }}
            >
              X
            </button>
          </td>
        </tr>
      );
    });
  };

  const mapServices = (services) => {
    return services.map((serv, index) => {
      return (
        <tr key={index}>
          <td>
            <select
              style={{ height: 30 }}
              onChange={(e) => {
                const arr = [...services];
                arr[index].fk_SERVICE = Number(e.target.value);
                setServices(arr);
              }}
              value={serv.fk_SERVICE}
            >
              {serviceList.map((elem) => {
                return (
                  <option
                    key={elem.id}
                    value={elem.id}
                  >{`${elem.name}`}</option>
                );
              })}
            </select>
          </td>
          <td>
            <ValidInput
              type="above zero number"
              value={serv.count}
              onChange={(e) => {
                const arr = [...services];
                arr[index].count = Number(e.target.value);
                setServices(arr);
              }}
              validation={handleValidation}
            />
          </td>
          <td>
            <button
              className="btn btn-sm btn-danger py-0 px-2 m-0 mr-3"
              disabled={services.length === 1}
              onClick={() => {
                if (serv.id !== 0)
                  setDeletedServices([...deletedServices, services[index]]);
                setServices(
                  services.filter((s, id) => {
                    return id !== index;
                  })
                );
              }}
            >
              X
            </button>
          </td>
        </tr>
      );
    });
  };

  const mapParts = (parts) => {
    return parts.map((part, index) => {
      return (
        <tr key={index}>
          <td>
            <select
              style={{ height: 30, minWidth: 174 }}
              onChange={(e) => {
                const arr = [...parts];
                arr[index].fk_PARTS = Number(e.target.value);
                setParts(arr);
              }}
              value={part.fk_PARTS}
            >
              {partList.map((elem) => {
                return (
                  <option
                    key={elem.id}
                    value={elem.id}
                  >{`${elem.name}`}</option>
                );
              })}
            </select>
          </td>
          <td>
            <ValidInput
              type="above zero number"
              value={part.count}
              onChange={(e) => {
                const arr = [...parts];
                arr[index].count = Number(e.target.value);
                setParts(arr);
              }}
              validation={handleValidation}
            />
          </td>
          <td>
            <button
              className="btn btn-sm btn-danger py-0 px-2 m-0 mr-3"
              onClick={() => {
                if (part.id !== 0)
                  setDeletedParts([...deletedParts, parts[index]]);
                setParts(
                  parts.filter((p, id) => {
                    return id !== index;
                  })
                );
              }}
            >
              X
            </button>
          </td>
        </tr>
      );
    });
  };

  return contract === null ? (
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
          Contract information
        </div>
        <table className="table table-borderless table-sm">
          <tbody>
            <tr>
              <td className="text-right">ID:</td>
              <td>{contract.id === "new" ? "-" : contract.id}</td>
            </tr>
            <tr>
              <td className="text-right">Order date:</td>
              <td>
                <ValidInput
                  type="date"
                  onChange={(e) => {
                    setContract({ ...contract, order_date: e.target.value });
                  }}
                  value={contract.order_date.substring(0, 10)}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">Repair start date:</td>
              <td>
                <ValidInput
                  type="date"
                  onChange={(e) => {
                    setContract({
                      ...contract,
                      repair_start_date: e.target.value,
                    });
                  }}
                  value={contract.repair_start_date.substring(0, 10)}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">Expected end date:</td>
              <td>
                <ValidInput
                  type="date"
                  onChange={(e) => {
                    setContract({
                      ...contract,
                      expected_end_date: e.target.value,
                    });
                  }}
                  value={contract.expected_end_date.substring(0, 10)}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">Real end date:</td>
              <td>
                <ValidInput
                  type="date"
                  onChange={(e) => {
                    setContract({
                      ...contract,
                      real_end_date: e.target.value,
                    });
                  }}
                  value={contract.real_end_date.substring(0, 10)}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">Worker:</td>
              <td>
                <select
                  style={{ height: 30 }}
                  onChange={(e) => {
                    setContract({
                      ...contract,
                      fk_WORKER: Number(e.target.value),
                    });
                  }}
                  value={contract.fk_WORKER}
                >
                  {mapPeople(workers)}
                </select>
              </td>
            </tr>
            <tr>
              <td className="text-right">Client:</td>
              <td>
                <select
                  style={{ height: 30 }}
                  onChange={(e) => {
                    setContract({
                      ...contract,
                      fk_CLIENT: Number(e.target.value),
                    });
                  }}
                  value={contract.fk_CLIENT}
                >
                  {mapPeople(clients)}
                </select>
              </td>
            </tr>
            <tr>
              <td className="text-right">Sum:</td>
              <td>
                <ValidInput
                  type="above zero number"
                  value={contract.sum}
                  onChange={(e) => {
                    setContract({
                      ...contract,
                      sum: Number(e.target.value),
                    });
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">Additional costs:</td>
              <td>
                <ValidInput
                  type="number"
                  value={contract.additional_costs}
                  onChange={(e) => {
                    setContract({
                      ...contract,
                      additional_costs: Number(e.target.value),
                    });
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div className="container border mt-3">
        <div
          style={{
            width: "fit-content",
            position: "relative",
            top: -14,
            backgroundColor: "#FFF",
          }}
          className="m-0 px-2"
        >
          Bills
        </div>
        <div className="row justify-content-center">
          <table className="table table-sm table-borderless w-auto mb-0">
            <tbody>
              <tr>
                <td style={{ minWidth: 184 }}>date</td>
                <td style={{ minWidth: 188 }}>sum</td>
                <td style={{ minWidth: 52 }}></td>
              </tr>
              {mapBills(bills)}
              <tr>
                <td>
                  <button
                    className="btn btn-primary btn-sm py-0"
                    onClick={() => {
                      const newBill = {
                        number: 0,
                        date: moment().format("YYYY-MM-DD"),
                        sum: "",
                        fk_CONTRACT: contract.id,
                      };
                      setBills([...bills, newBill]);
                    }}
                  >
                    add
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <div className="container border mt-3">
        <div
          style={{
            width: "fit-content",
            position: "relative",
            top: -14,
            backgroundColor: "#FFF",
          }}
          className="m-0 px-2"
        >
          Ordered services
        </div>
        <div className="row justify-content-center">
          <table className="table table-sm table-borderless w-auto mb-0">
            <tbody>
              <tr>
                <td style={{ minWidth: 184 }}>service</td>
                <td style={{ minWidth: 188 }}>count</td>
                <td style={{ minWidth: 52 }}></td>
              </tr>
              {mapServices(services)}
              <tr>
                <td>
                  <button
                    className="btn btn-primary btn-sm py-0"
                    onClick={() => {
                      const newServ = {
                        id: 0,
                        count: 1,
                        fk_SERVICE: 1,
                        fk_CONTRACT: ownProps.match.params.id,
                      };
                      setServices([...services, newServ]);
                    }}
                  >
                    add
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <div className="container border mt-3">
        <div
          style={{
            width: "fit-content",
            position: "relative",
            top: -14,
            backgroundColor: "#FFF",
          }}
          className="m-0 px-2"
        >
          Used parts
        </div>
        <div className="row justify-content-center">
          <table className="table table-sm table-borderless w-auto mb-0">
            <tbody>
              <tr>
                <td style={{ minWidth: 184 }}>part</td>
                <td style={{ minWidth: 188 }}>count</td>
                <td style={{ minWidth: 52 }}></td>
              </tr>
              {mapParts(parts)}
              <tr>
                <td>
                  <button
                    className="btn btn-primary btn-sm py-0"
                    onClick={() => {
                      const newPart = {
                        id: 0,
                        count: 1,
                        fk_PARTS: 1,
                        fk_CONTRACT: ownProps.match.params.id,
                      };
                      setParts([...parts, newPart]);
                    }}
                  >
                    add
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <div className="row justify-content-center mt-3">
        <button
          className="btn btn-primary"
          disabled={
            !areChanges ||
            !isValid ||
            workers.length === 0 ||
            clients.length === 0 ||
            (services.length > 0 && serviceList.length === 0) ||
            (parts.length > 0 && partList.length === 0)
          }
          onClick={() => {
            Axios.post("http://localhost:5000/api/sql", {
              query: `INSERT INTO contract(id, order_date, repair_start_date, expected_end_date, real_end_date, sum, additional_costs, fk_WORKER, fk_CLIENT) 
                      VALUES (${contract.id}, '${contract.order_date}', '${contract.repair_start_date}',
                              '${contract.expected_end_date}', '${contract.real_end_date}', 
                              '${contract.sum}', '${contract.additional_costs}', 
                              '${contract.fk_WORKER}', '${contract.fk_CLIENT}')
                      ON DUPLICATE KEY UPDATE 
                      order_date='${contract.order_date}', 
                      repair_start_date='${contract.repair_start_date}', 
                      expected_end_date='${contract.expected_end_date}', 
                      real_end_date='${contract.real_end_date}', 
                      sum='${contract.sum}', 
                      additional_costs='${contract.additional_costs}', 
                      fk_WORKER='${contract.fk_WORKER}', 
                      fk_CLIENT='${contract.fk_CLIENT}'`,
            }).then((res) => {
              let axios = [];
              if (deletedBills.length > 0)
                axios = [
                  ...axios,
                  Axios.post("http://localhost:5000/api/sql", {
                    query: `DELETE FROM bill 
                      WHERE bill.number IN (${deletedBills
                        .map((d) => {
                          return d.number;
                        })
                        .join(", ")})`,
                  }),
                ];
              if (bills.length > 0)
                axios = [
                  ...axios,
                  Axios.post("http://localhost:5000/api/sql", {
                    query: `INSERT INTO bill(number, date, sum, fk_CONTRACT) 
                      (VALUES ${bills
                        .map(
                          (d) =>
                            `(${d.number}, '${d.date}', '${d.sum}', ${res.data.insertId}) `
                        )
                        .join(", ")}) 
                        ON DUPLICATE KEY UPDATE 
                        date=VALUES(date), 
                        sum=VALUES(sum), 
                        fk_CONTRACT=VALUES(fk_CONTRACT) `,
                  }),
                ];
              if (deletedServices.length > 0)
                axios = [
                  ...axios,
                  Axios.post("http://localhost:5000/api/sql", {
                    query: `DELETE FROM ordered_service 
                      WHERE ordered_service.id IN (${deletedServices
                        .map((s) => {
                          return s.id;
                        })
                        .join(", ")})`,
                  }),
                ];
              if (services.length > 0)
                axios = [
                  ...axios,
                  Axios.post("http://localhost:5000/api/sql", {
                    query: `INSERT INTO ordered_service(id, count, fk_SERVICE, fk_CONTRACT) 
                      (VALUES ${services
                        .map(
                          (d) =>
                            `(${d.id}, '${d.count}', '${d.fk_SERVICE}', ${res.data.insertId}) `
                        )
                        .join(", ")}) 
                        ON DUPLICATE KEY UPDATE 
                        count=VALUES(count), 
                        fk_SERVICE=VALUES(fk_SERVICE), 
                        fk_CONTRACT=VALUES(fk_CONTRACT) `,
                  }),
                ];
              if (deletedParts.length > 0)
                axios = [
                  ...axios,
                  Axios.post("http://localhost:5000/api/sql", {
                    query: `DELETE FROM parts_used 
                      WHERE parts_used.id IN (${deletedParts
                        .map((p) => {
                          return p.id;
                        })
                        .join(", ")})`,
                  }),
                ];
              if (parts.length > 0)
                axios = [
                  ...axios,
                  Axios.post("http://localhost:5000/api/sql", {
                    query: `INSERT INTO parts_used(id, count, fk_PARTS, fk_CONTRACT) 
                      (VALUES ${parts
                        .map(
                          (d) =>
                            `(${d.id}, '${d.count}', '${d.fk_PARTS}', ${res.data.insertId}) `
                        )
                        .join(", ")}) 
                        ON DUPLICATE KEY UPDATE 
                        count=VALUES(count), 
                        fk_PARTS=VALUES(fk_PARTS), 
                        fk_CONTRACT=VALUES(fk_CONTRACT) `,
                  }),
                ];
              Axios.all(axios)
                .then(() => {
                  history.push("/contracts");
                })
                .catch((e) => {});
            });
          }}
        >
          save
        </button>
      </div>
    </div>
  );
};

export default Contract;
