var mysql = require("mysql");
module.exports = conn = mysql.createConnection({
  host: "localhost",
  port: 3306,
  user: "root",
  database: "evamik3",
  dateStrings: ["DATE", "DATETIME"],
});

conn.connect((err) => {
  if (err) throw err;
  console.log("mysql connected");
});
