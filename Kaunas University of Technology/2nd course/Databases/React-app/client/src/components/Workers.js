import React from "react";
import TableComponent from "./TableComponent";
import RowButtons from "./RowButtons";
import Axios from "axios";

const Workers = () => {
  const query = `SELECT worker.name, worker.surname, 
                worker.id, 
                garage.name as 'garage', 
                worker_status.name as 'status' 
                FROM worker
                LEFT JOIN garage ON worker.fk_GARAGE=garage.id 
                LEFT JOIN worker_status on worker.worker_status=worker_status.id`;

  const headers = ["ID", "Worker", "Status", "Workplace"];

  const removeElement = (index, setElements, elements) => {
    setElements(
      elements.filter((e, id) => {
        return id !== index;
      })
    );
  };

  const mapElements = (elements, setElements) => {
    return elements.map((el, index) => {
      return (
        <tr key={el.id}>
          <td>{el.id}</td>
          <td>
            {el.name} {el.surname}
          </td>
          <td>{el.status}</td>
          <td>{el.garage}</td>
          <RowButtons
            link={`/workers/id=${el.id}`}
            onClick={() => {
              Axios.post("http://localhost:5000/api/sql", {
                query: `DELETE FROM worker WHERE worker.id=${el.id}`,
              }).then((res) => {
                removeElement(index, setElements, elements);
              });
            }}
          />
        </tr>
      );
    });
  };

  return (
    <TableComponent
      query={query}
      headers={headers}
      mapElements={mapElements}
      removeElement={removeElement}
      newLink={"/workers/id=new"}
    />
  );
};

export default Workers;
