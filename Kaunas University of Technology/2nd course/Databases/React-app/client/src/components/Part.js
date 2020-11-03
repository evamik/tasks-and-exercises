import React, { useEffect, useState } from "react";
import Axios from "axios";
import { history } from "../redux/history";
import moment from "moment";
import ValidInput from "./ValidInput";

const Worker = (ownProps) => {
  const [part, setPart] = useState(null);
  const [defaultPart, setDefaultPart] = useState(null);
  const [areChanges, setChanges] = useState(false);

  const [isValid, setValid] = useState(true);
  const [invalidFields, setInvalidFields] = useState(0);

  const [garages, setGarages] = useState([]);

  useEffect(() => {
    Axios.post("http://localhost:5000/api/sql", {
      query: `SELECT * FROM garage`,
    }).then((res1) => {
      setGarages([...res1.data]);

      let garage = 1;
      if ([...res1.data].length > 0) garage = [...res1.data][0].id;

      if (ownProps.match.params.id === "new") {
        const work = {
          id: 0,
          name: "",
          price: "",
          count: "",
          fk_GARAGE: garage,
        };

        setPart(work);
        setDefaultPart({ ...work });
        return;
      }

      Axios.post("http://localhost:5000/api/sql", {
        query: `SELECT * FROM parts 
            WHERE parts.id=${ownProps.match.params.id}`,
      }).then((res2) => {
        setPart(res2.data[0]);
        setDefaultPart(res2.data[0]);
      });
    });
  }, [ownProps.match.params.id]);

  useEffect(() => {
    if (JSON.stringify(part) !== JSON.stringify(defaultPart)) setChanges(true);
    else setChanges(false);
  }, [part]);

  const handleValidation = (valid) => {
    if (valid) {
      setValid(invalidFields - 1 === 0);
      setInvalidFields(invalidFields - 1);
    } else {
      setValid(false);
      setInvalidFields(invalidFields + 1);
    }
  };

  return part === null ? (
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
              <td>{part.id === "new" ? "-" : part.id}</td>
            </tr>
            <tr>
              <td className="text-right">name:</td>
              <td>
                <ValidInput
                  type="text"
                  value={part.name}
                  onChange={(e) => {
                    setPart({
                      ...part,
                      name: e.target.value,
                    });
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">price:</td>
              <td>
                <ValidInput
                  type="above zero number"
                  value={part.price}
                  onChange={(e) => {
                    setPart({
                      ...part,
                      price: Number(e.target.value),
                    });
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">count:</td>
              <td>
                <ValidInput
                  type="number"
                  value={part.count}
                  onChange={(e) => {
                    setPart({
                      ...part,
                      count: Number(e.target.value),
                    });
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">garage:</td>
              <td>
                <select
                  style={{ height: 30 }}
                  onChange={(e) => {
                    setPart({
                      ...part,
                      fk_GARAGE: Number(e.target.value),
                    });
                  }}
                  value={part.fk_GARAGE}
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
          disabled={!areChanges || !isValid || garages.length === 0}
          onClick={() => {
            Axios.post("http://localhost:5000/api/sql", {
              query: `INSERT INTO parts(id, name, price, count, fk_GARAGE) 
                      VALUES (${part.id}, '${part.name}', '${part.price}',
                              '${part.count}', '${part.fk_GARAGE}')
                      ON DUPLICATE KEY UPDATE 
                      name='${part.name}', 
                      price='${part.price}', 
                      count='${part.count}', 
                      fk_GARAGE='${part.fk_GARAGE}'`,
            })
              .then((res) => {
                history.push("/parts");
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
