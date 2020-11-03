import React from "react";
import { history } from "../redux/history";

const RowButtons = (props) => {
  return (
    <td className="row justify-content-end">
      <button
        onClick={() => history.push(props.link)}
        className="btn btn-sm btn-info py-0 px-2 m-0 mr-1"
      >
        edit
      </button>
      <button
        onClick={() => props.onClick()}
        className="btn btn-sm btn-danger py-0 px-2 m-0 mr-3"
      >
        X
      </button>
    </td>
  );
};

export default RowButtons;
