import React, { useState, useEffect } from "react";

const ValidInput = (props) => {
  const [isValid, setValid] = useState(true);
  const [validationMsg, setValidationMsg] = useState(
    "This field can't be empty."
  );
  const [type, setType] = useState("text");

  const handleValidation = () => {
    let bool = isValid;
    switch (props.type) {
      case "date":
        bool = new Date(props.value).toString() !== "Invalid Date";
        break;
      case "number":
        bool = !isNaN(props.value) && props.value.length !== 0;
        break;
      case "above zero number":
        bool =
          !isNaN(props.value) && props.value.length !== 0 && props.value > 0;
        break;
      default:
        bool = props.value.length !== 0;
        break;
    }

    if (isValid !== bool) {
      setValid(bool);
      if (props.validation) {
        props.validation(bool);
      }
    }
  };

  useEffect(() => {
    handleValidation();
  }, [props.value]);

  useEffect(() => {
    switch (props.type) {
      case "date":
        setType("date");
        setValidationMsg("Incorrect date format");
        break;
      case "number":
        setType("number");
        break;
      case "above zero number":
        setType("number");
        setValidationMsg("This field can't be zero");
        break;
      default:
        break;
    }
  }, []);

  useEffect(
    () => () => {
      if (!isValid) {
        if (props.validation) {
          props.validation(true);
        }
      }
    },
    [isValid]
  );

  return (
    <div>
      <input type={type} value={props.value} onChange={props.onChange} />
      {isValid ? (
        <div></div>
      ) : (
        <div className="text-danger">{validationMsg}</div>
      )}
    </div>
  );
};

export default ValidInput;
