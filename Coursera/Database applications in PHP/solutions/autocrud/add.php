<?php
session_start();
if (!isset($_SESSION['name'])) {
    die("ACCESS DENIED");
}

require_once "pdo.php";

if (isset($_POST['add']) && isset($_POST['mileage']) && isset($_POST['year']) && isset($_POST['make']) && isset($_POST['model'])) {
    if ($_POST['make'] == "" || $_POST['mileage'] == "" || $_POST['year'] == "" || $_POST['model'] == "") {
        $_SESSION['failure'] = "All fields are required";
        header("Location: add.php");
        return;
    } else if (!is_numeric($_POST['year']) && !is_numeric($_POST['mileage'])) {
        $_SESSION['failure'] = "Mileage and year must be numeric";
        header("Location: add.php");
        return;
    } else if (!is_numeric($_POST['year'])) {
        $_SESSION['failure'] = "Year must be numeric";
        header("Location: add.php");
        return;
    } else if (!is_numeric($_POST['mileage'])) {
        $_SESSION['failure'] = "Mileage must be numeric";
        header("Location: add.php");
        return;
    } else {
        $sql = "INSERT INTO autos (make, year, mileage, model) VALUES(:make, :year, :mileage, :model)";
        $stmt = $pdo->prepare($sql);
        $stmt->execute(array(
            ':make' => $_POST['make'],
            ':year' => $_POST['year'],
            'mileage' => $_POST['mileage'],
            'model' => $_POST['model']
        ));
        $_SESSION['success'] = "Record added";
        header("Location: index.php");
        return;
    }
}
?>
<!DOCTYPE html>
<html>

<head>
    <title>Evaldas Mikalauskas's Automobile Tracker</title>

    <?php require_once "bootstrap.php"; ?>

</head>

<body>
    <div class="container">
        <h1>Tracking Autos for <?= htmlentities($_SESSION['name']) ?></h1>
        <?php
        if (isset($_SESSION['failure'])) {
            echo ('<p style="color: red;">' . htmlentities($_SESSION['failure']) . "</p>\n");
            unset($_SESSION['failure']);
        }
        ?>
        <form method="post">
            <p>Make: <input type="text" name="make" size="60"></p>
            <p>Model: <input type="text" name="model" size="60"></p>
            <p>Year: <input type="text" name="year"></p>
            <p>Mileage: <input type="text" name="mileage"></p>
            <input type="submit" name="add" value="Add">
            <input type="submit" name="cancel" value="Cancel">
        </form>
    </div>

</html>