import React from "react";
import TableComponent from "./TableComponent";
import RowButtons from "./RowButtons";
import Axios from "axios";

const Contract = () => {
  const query = `SELECT contract.id, contract.order_date, 
                worker.name AS 'workername', 
                worker.surname AS 'workersurname', 
                client.name AS 'clientname', 
                client.surname AS 'clientsurname'
                FROM contract
                LEFT JOIN worker ON contract.fk_WORKER=worker.id
                LEFT JOIN client ON contract.fk_CLIENT=client.personal_code`;

  const headers = ["ID", "Date", "Worker", "Client"];

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
          <td>{el.order_date.substring(0, 10)}</td>
          <td>
            {el.workername} {el.workersurname}
          </td>
          <td>
            {el.clientname} {el.clientsurname}
          </td>
          <RowButtons
            link={`/contracts/id=${el.id}`}
            onClick={() => {
              Axios.post("http://localhost:5000/api/sql", {
                query: `DELETE FROM contract WHERE contract.id=${el.id}`,
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
      newLink={"/contracts/id=new"}
    />
  );
};

export default Contract;
