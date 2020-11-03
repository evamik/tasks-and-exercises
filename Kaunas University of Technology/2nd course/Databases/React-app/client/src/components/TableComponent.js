import React, { useEffect, useState } from "react";
import Axios from "axios";
import { history } from "../redux/history";

const TableComponent = (props) => {
  const [elements, setElements] = useState([]);
  const [total, setTotal] = useState(0);

  useEffect(() => {
    Axios.post("http://localhost:5000/api/sql", {
      query: props.query,
    })
      .then((res) => {
        setElements(props.mapTotal ? res.data.slice(0, -1) : res.data);
        if (props.mapTotal) setTotal(res.data[res.data.length - 1].count);
      })
      .catch((err) => {
        console.log(err);
      });
  }, [props.query]);

  function mapTableHeader() {
    return props.headers.map((header, index) => {
      return <th key={index}>{header}</th>;
    });
  }

  return (
    <div>
      <table className="table table-light table-sm table-striped">
        <thead className="thead-dark bg-dark">
          <tr>
            {mapTableHeader()}
            <th className=" text-right">
              {props.reportLink ? (
                <button
                  className="btn btn-primary btn-sm py-0 mr-2"
                  onClick={() => history.push(props.reportLink)}
                >
                  report
                </button>
              ) : null}
              {props.newLink ? (
                <button
                  className="btn btn-primary btn-sm py-0"
                  onClick={() => history.push(props.newLink)}
                >
                  new
                </button>
              ) : null}
            </th>
          </tr>
        </thead>
        <tbody>
          {props.mapElements(elements, setElements)}
          {props.mapTotal ? props.mapTotal(total) : null}
        </tbody>
      </table>
    </div>
  );
};

export default TableComponent;
