import React, { useEffect, useState } from "react";
import Axios from "axios";
import { history } from "../redux/history";
import moment from "moment";
import ValidInput from "./ValidInput";

const Service = (ownProps) => {
  const [service, setService] = useState(null);
  const [defaultService, setDefaultService] = useState(null);
  const [areChanges, setChanges] = useState(false);

  const [isValid, setValid] = useState(true);
  const [invalidFields, setInvalidFields] = useState(0);

  useEffect(() => {
    if (ownProps.match.params.id === "new") {
      const serv = {
        id: 0,
        name: "",
        price: "",
        duration_days: "",
      };

      setService(serv);
      setDefaultService({ ...serv });
      return;
    }

    Axios.post("http://localhost:5000/api/sql", {
      query: `SELECT * FROM service 
            WHERE service.id=${ownProps.match.params.id}`,
    }).then((res) => {
      setService(res.data[0]);
      setDefaultService(res.data[0]);
      console.log(res.data[0]);
    });
  }, [ownProps.match.params.id]);

  useEffect(() => {
    if (JSON.stringify(service) !== JSON.stringify(defaultService))
      setChanges(true);
    else setChanges(false);
  }, [service]);

  const handleValidation = (valid) => {
    if (valid) {
      setValid(invalidFields - 1 === 0);
      setInvalidFields(invalidFields - 1);
    } else {
      setValid(false);
      setInvalidFields(invalidFields + 1);
    }
  };

  return service === null ? (
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
          Service information
        </div>
        <table className="table table-borderless table-sm">
          <tbody>
            <tr>
              <td className="text-right">ID:</td>
              <td>{service.id === "new" ? "-" : service.id}</td>
            </tr>
            <tr>
              <td className="text-right">name:</td>
              <td>
                <ValidInput
                  type="text"
                  value={service.name}
                  onChange={(e) => {
                    setService({
                      ...service,
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
                  value={service.price}
                  onChange={(e) => {
                    setService({
                      ...service,
                      price: Number(e.target.value),
                    });
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">duration in days:</td>
              <td>
                <ValidInput
                  type="above zero number"
                  value={service.duration_days}
                  onChange={(e) => {
                    setService({
                      ...service,
                      duration_days: Number(e.target.value),
                    });
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div className="row justify-content-center mt-3">
        <button
          className="btn btn-primary"
          disabled={!areChanges || !isValid}
          onClick={() => {
            Axios.post("http://localhost:5000/api/sql", {
              query: `INSERT INTO service(id, name, price, duration_days) 
                      VALUES (${service.id}, '${service.name}', '${service.price}',
                              '${service.duration_days}')
                      ON DUPLICATE KEY UPDATE 
                      name='${service.name}', 
                      price='${service.price}', 
                      duration_days='${service.duration_days}'`,
            })
              .then((res) => {
                history.push("/services");
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

export default Service;
