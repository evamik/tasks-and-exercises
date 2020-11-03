import React from "react";
import TableComponent from "./TableComponent";
import RowButtons from "./RowButtons";
import Axios from "axios";

const Parts = () => {
  const query = `SELECT parts.id, parts.name, parts.price, 
                parts.count, garage.name as 'garage'
                FROM parts 
                LEFT JOIN garage ON parts.fk_GARAGE=garage.id`;

  const headers = ["ID", "Name", "Price", "Count", "Garage"];

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
          <td>{el.name}</td>
          <td>{el.price}</td>
          <td>{el.count}</td>
          <td>{el.garage}</td>
          <RowButtons
            link={`/parts/id=${el.id}`}
            onClick={() => {
              Axios.post("http://localhost:5000/api/sql", {
                query: `DELETE FROM parts WHERE parts.id=${el.id}`,
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
      newLink={"/parts/id=new"}
    />
  );
};

export default Parts;
