import React from "react";
import TableComponent from "./TableComponent";
import RowButtons from "./RowButtons";
import Axios from "axios";

const Services = () => {
  const query = `SELECT * FROM service`;

  const headers = ["ID", "Name", "Price", "Duration in days"];

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
          <td>{el.duration_days}</td>
          <RowButtons
            link={`/services/id=${el.id}`}
            onClick={() => {
              Axios.post("http://localhost:5000/api/sql", {
                query: `DELETE FROM service WHERE service.id=${el.id}`,
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
      reportLink={"/services/report"}
      newLink={"/services/id=new"}
    />
  );
};

export default Services;
