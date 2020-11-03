import React from "react";
import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import { connect } from "react-redux";
import Navbar from "./components/Navbar";

class App extends React.Component {
  render() {
    return (
      <div>
        <Navbar />
        <div className="container">{this.props.children}</div>
      </div>
    );
  }
}

export default connect(null)(App);
