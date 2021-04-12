import { decode } from "html-entities";
import React, { useEffect, useState } from "react";
import {
  Button,
  Col,
  Dropdown,
  DropdownButton,
  Form,
  InputGroup,
  Row,
} from "react-bootstrap";
import { BTCService } from "../../services/BTCService";

const CurrencyPrepend = ({ code }) => {
  return (
    <InputGroup.Prepend style={{ minWidth: "4em" }}>
      <InputGroup.Text className="w-100 justify-content-end">
        {code}
      </InputGroup.Text>
    </InputGroup.Prepend>
  );
};

const CurrencyInput = ({ code, value, handleHide }) => {
  const onClick = () => {
    handleHide(code);
  };

  return (
    <Form.Group>
      <InputGroup>
        <CurrencyPrepend code={code} />
        <Form.Control readOnly type="text" value={value} />
        <InputGroup.Append>
          <Button onClick={onClick} variant="secondary">
            X
          </Button>
        </InputGroup.Append>
      </InputGroup>
    </Form.Group>
  );
};

const BTCForm = () => {
  const [bitcoins, setBitcoins] = useState(0);
  const [currencies, setCurrencies] = useState();
  const [showedCurrencies, setShowedCurrencies] = useState();
  const [hiddenCount, setHiddenCount] = useState(0);

  useEffect(() => {
    getRates(true);
    setInterval(getRates, 60000);
  }, []);

  const getRates = (setShowed) => {
    BTCService.getRates().then((res) => {
      setShowed &&
        setShowedCurrencies(
          Object.assign(
            {},
            ...Object.values(res.bpi).map((cur) => ({ [cur.code]: true }))
          )
        );
      setCurrencies(res.bpi);
    });
  };

  const handleChange = (e) => {
    setBitcoins(e.target.value);
  };

  const handleHide = (code) => {
    setShowedCurrencies({ ...showedCurrencies, [code]: false });
    setHiddenCount(hiddenCount + 1);
  };

  const handleShow = (code) => {
    setShowedCurrencies({ ...showedCurrencies, [code]: true });
    setHiddenCount(hiddenCount - 1);
  };

  return (
    <Row className="d-flex flex-column">
      <Col xs={12} md={6}>
        <Form.Group>
          <InputGroup>
            <CurrencyPrepend code="BTC" />
            <Form.Control
              type="number"
              defaultValue={0}
              onChange={handleChange}
            />
          </InputGroup>
        </Form.Group>
        {hiddenCount > 0 && (
          <DropdownButton className="mb-3" title="Add currency">
            {Object.keys(showedCurrencies).map((code) => {
              return !showedCurrencies[code] ? (
                <Dropdown.Item key={code} onSelect={() => handleShow(code)}>
                  {code}, {currencies[code].description}
                </Dropdown.Item>
              ) : null;
            })}
          </DropdownButton>
        )}
        {currencies &&
          Object.keys(showedCurrencies).map((code) => {
            return showedCurrencies[code] ? (
              <CurrencyInput
                key={code}
                code={code}
                value={(
                  decode(currencies[code].symbol) +
                  (currencies[code].rate_float * bitcoins).toFixed(2)
                )
                  .toString()
                  .replace(/\B(?=(\d{3})+(?!\d))/g, ",")}
                handleHide={handleHide}
              />
            ) : null;
          })}
      </Col>
    </Row>
  );
};

export default BTCForm;
