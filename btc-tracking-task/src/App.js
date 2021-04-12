import React from "react";
import { Container } from "react-bootstrap";
import BTCForm from "./components/btc-page/BTCForm";

const App = () => {
  return (
    <Container className="py-5">
      <BTCForm />
    </Container>
  );
};

export default App;
