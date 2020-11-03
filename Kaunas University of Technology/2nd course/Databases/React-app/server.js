require("dotenv").config();
const express = require("express");
const cors = require("cors");
const bodyParser = require("body-parser");
const path = require("path");
const db = require("./database");

const app = express();
app.use(cors(), bodyParser.urlencoded({ extended: true }), bodyParser.json());
const port = process.env.PORT || 5000;
const http = require("http").createServer(app);

app.use(express.json());

app.post("/api/sql", (req, res) => {
  db.query(req.body.query, (err, result) => {
    if (err) {
      console.log(err);
      res.sendStatus(400);
    }

    res.json(result);
  });
});

http.listen(port, () => {
  console.log(`listening on ${port}`);
});

if (process.env.NODE_ENV === "production") {
  // Set a static folder
  app.use(express.static("client/build"));
  app.get("*", (req, res) =>
    res.sendFile(path.resolve(__dirname, "client", "build", "index.html"))
  );
}
