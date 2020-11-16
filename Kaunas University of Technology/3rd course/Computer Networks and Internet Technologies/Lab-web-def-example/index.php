<?php
$server = "localhost";
$user = "root";
$password = "root";
$dbname = "stud";
$lentele = "laboratorinis";
$IP = $_SERVER['REMOTE_ADDR'];

// prisijungti
$conn = new mysqli($server, $user, $password, $dbname);
if ($conn->connect_error) die("Negaliu prisijungti: " . $conn->connect_error);

if ($_POST != null) {
    $vardas = $_POST['vardas'];
    $epastas = $_POST['epastas'];
    $kam = $_POST['kam'];
    $z = $_POST['zinute'];
    $lytis = $_POST['lytis'];

    //$zinute = $_POST['zinute'];

    $sql = "INSERT INTO $lentele (vardas, epastas, kam, data,IP,Zinute,lytis)
          VALUES ('$vardas', '$epastas','$kam', NOW(),'$IP','$z', '$lytis')";
    if (!$result = $conn->query($sql)) die("Negaliu įrašyti: " . $conn->error);
    //echo "Įrašyta";
    else {
        header("Location:index.php");
        $conn->close();
        exit();
    }
}


?>
<!DOCTYPE html>
<html>
<link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js">
</script>
<script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js">
</script>

<style>
    #zinutes {
        font-family: Arial;
        border-collapse: collapse;
        width: 70%;
    }

    #zinutes td {
        border: 1px solid #ddd;
        padding: 8px;
    }

    #zinutes tr:nth-child(even) {
        background-color: #f2f2f2;
    }

    #zinutes tr:hover {
        background-color: #ddd;
    }
</style>

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Php.lab</title>
</head>

<body>
    <h3>Žinučių sistema</h3>
    <div class="container">
        <form method='get'>
            <div class="form-group col-lg-12">
                <label for="filtras" class="control-label">Filtras:</label>
                <select name="filtras" class="form-control input-sm">
                    <option value="" <?php if ($_GET['filtras'] == "") echo 'selected'; ?>>Visi įrašai</option>
                    <option value="Vyras" <?php if ($_GET['filtras'] == "Vyras") echo 'selected'; ?>>Vyras</option>
                    <option value="Moteris" <?php if ($_GET['filtras'] == "Moteris") echo 'selected'; ?>>Moteris</option>
                </select>
            </div>
            <div class="form-group col-lg-2">
                <input type='submit' name='ok' value='filtruoti' class="btn btn-default">
            </div>
        </form>
    </div>
    <table style="margin: 0px auto;" id="zinutes">

        <?php

        //  nuskaityti
        if ($_GET['filtras'] == "")
            $sql =  "SELECT * FROM $lentele";
        else {
            $filtras = $_GET['filtras'];
            $sql =  "SELECT * FROM $lentele WHERE lytis='$filtras'";
        }
        if (!$result = $conn->query($sql)) die("Negaliu nuskaityti: " . $conn->error);

        // parodyti
        //echo "<table border=\"1\">";
        echo "<tr>
                <td>id</td>
                <td>vardas</td>
                <td>epastas</td>
                <td>gavejas</td>
                <td>Data(IP)</td>
                <td>Zinute</td>
                <td>Lytis</td>
            </tr>";
        while ($row = $result->fetch_assoc()) {
            echo "<tr>
                <td>" . $row['id'] . "</td>
                <td>" . $row['vardas'] . "</td>
                <td>" . $row['epastas'] . "</td>
                <td>" . $row['kam'] . "</td>
                <td>" . $row['data'] . " (" . $row['IP'] . ")</td>
                <td>" . $row['Zinute'] . "</td>
                <td>" . $row['lytis'] . "</td>
            </tr>";
        }
        //echo "</table>";
        $conn->close();

        ?>
    </table>

    <div class="container">
        <form method='post'>
            <div class="form-group col-lg-4">
                <label for="vardas" class="control-label">Siuntėjo vardas:</label>
                <input name='vardas' type='text' class="form-control input-sm">
            </div>
            <div class="form-group col-lg-4">
                <label for="epastas" class="control-label">Siuntėjo e-paštas:</label>
                <input name='epastas' id="epastas" type='email' class="form-control input-sm">
            </div>
            <div class="form-group col-lg-4">
                <label for="kam" class="control-label">Kam skirta:</label>
                <input name='kam' type='text' class="form-control input-sm">
            </div>
            <div class="form-group col-lg-12">
                <label for="zinute" class="control-label">Žinutė:</label>
                <textarea name='zinute' class="form-control input-sm"></textarea>
            </div>
            <div class="form-group col-lg-12">
                <label for="lytis" class="control-label">Lytis:</label>
                <select name="lytis" class="form-control input-sm">
                    <option selected>Pasirinkite lytį</option>
                    <option value="Vyras">Vyras</option>
                    <option value="Moteris">Moteris</option>
                </select>
            </div>
            <div class="form-group col-lg-2">
                <input type='submit' name='ok' value='siųsti' class="btn btn-default">
            </div>
        </form>
    </div>
</body>

</html>