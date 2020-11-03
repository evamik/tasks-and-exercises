import React, { useEffect, useState, useRef } from "react";
import Axios from "axios";
import { history } from "../redux/history";
import moment from "moment";
import ValidInput from "./ValidInput";
import ReportTable from "./ReportTable";

const Report = (ownProps) => {
  const [isValid, setValid] = useState(true);
  const [invalidFields, setInvalidFields] = useState(0);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [showReport, setShowReport] = useState(false);

  let localInvalidFields = invalidFields;

  const handleValidation = (valid) => {
    if (valid) {
      localInvalidFields--;
      setInvalidFields(localInvalidFields);
      setValid(localInvalidFields === 0);
    } else {
      setValid(false);
      localInvalidFields++;
      setInvalidFields(localInvalidFields);
    }
  };

  return !showReport ? (
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
          Parts report form
        </div>
        <table className="table table-borderless table-sm">
          <tbody>
            <tr>
              <td className="text-right">date from:</td>
              <td>
                <ValidInput
                  type="date"
                  value={startDate}
                  onChange={(e) => {
                    setStartDate(e.target.value);
                  }}
                  validation={handleValidation}
                />
              </td>
            </tr>
            <tr>
              <td className="text-right">date to:</td>
              <td>
                <ValidInput
                  type="date"
                  value={endDate}
                  onChange={(e) => {
                    setEndDate(e.target.value);
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
          disabled={!isValid}
          onClick={() => {
            setShowReport(true);
          }}
        >
          show report
        </button>
      </div>
    </div>
  ) : (
    <div>
      <ReportTable startDate={startDate} endDate={endDate} />
      <div className="row justify-content-center mt-3">
        <button
          className="btn btn-primary"
          disabled={!isValid}
          onClick={() => {
            setShowReport(false);
          }}
        >
          new report
        </button>
      </div>
    </div>
  );
};

export default Report;
