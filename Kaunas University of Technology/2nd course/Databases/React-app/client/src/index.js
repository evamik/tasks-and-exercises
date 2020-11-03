import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter } from "react-router-dom";
import configureStore from "./redux/configureStore";
import { Provider as ReduxProvider } from "react-redux";
import routes from "./routes";

const store = configureStore();

ReactDOM.render(
  <ReduxProvider store={store}>
    <BrowserRouter>{routes}</BrowserRouter>
  </ReduxProvider>,
  document.getElementById("root")
);
