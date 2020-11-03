import React from "react";
import TableComponent from "./TableComponent";
import RowButtons from "./RowButtons";
import Axios from "axios";

const ReportTable = (props) => {
  const query = ` SELECT  service.id, 
                          service.name, 
                          IFNULL(a.count, 0) as 'count'
                  FROM service 
                    LEFT JOIN (
                      SELECT  ordered_service.fk_SERVICE as service, 
                              SUM(IF(YEAR(contract.order_date)>='${props.startDate}'
                                AND YEAR(contract.order_date)<'${props.endDate}', ordered_service.count, NULL)) as count
                      FROM ordered_service
                      LEFT JOIN contract ON contract.id = ordered_service.fk_CONTRACT
                      GROUP BY service
                    ) a ON a.service = service.id
                  UNION SELECT ${Number.MAX_SAFE_INTEGER}, NULL, IFNULL(SUM(IF(YEAR(contract.order_date)>='${props.startDate}'
                            AND YEAR(contract.order_date)<'${props.endDate}', ordered_service.count, NULL)), 0) as count
                  FROM ordered_service
                  LEFT JOIN contract ON contract.id = ordered_service.fk_CONTRACT
                  ORDER BY id`;

  const headers = ["ID", "Name", "Count"];

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
          <td>{el.count}</td>
          <td></td>
        </tr>
      );
    });
  };

  const mapTotal = (total) => {
    return (
      <tr>
        <td></td>
        <td></td>
        <td>{total}</td>
        <td></td>
      </tr>
    );
  };

  return (
    <TableComponent
      query={query}
      headers={headers}
      mapElements={mapElements}
      mapTotal={mapTotal}
    />
  );
};

export default ReportTable;
