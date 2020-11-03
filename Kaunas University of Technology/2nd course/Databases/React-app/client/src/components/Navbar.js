import React from "react";

const Navbar = () => {
  return (
    <nav className="navbar sticky-top navbar-dark bg-light">
      <div className="row justify-content-left mx-1">
        <a className="btn btn-info py-1 mx-1" href="/contracts">
          Contracts
        </a>
        <a className="btn btn-info py-1 mx-1" href="/workers">
          Workers
        </a>
        <a className="btn btn-info py-1 mx-1" href="/services">
          Services
        </a>
        <a className="btn btn-info py-1 mx-1" href="/parts">
          Parts
        </a>
      </div>
    </nav>
  );
};

export default Navbar;
