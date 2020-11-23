<?php
session_start();
if (!isset($_SESSION['name'])) {
    die("Not logged in");
}

if (isset($_POST['cancel'])) {
    header("Location: view.php");
    return;
}

require_once "pdo.php";

if (isset($_POST['add']) && isset($_POST['mileage']) && isset($_POST['year']) && isset($_POST['make'])) {
    if ($_POST['make'] == "") {
        $_SESSION['failure'] = "Make is required";
        header("Location: add.php");
        return;
    } else if (!is_numeric($_POST['year']) || !is_numeric($_POST['mileage'])) {
        $_SESSION['failure'] = "Mileage and year must be numeric";
        header("Location: add.php");
        return;
    } else {
        $sql = "INSERT INTO autos (make, year, mileage) VALUES(:make, :year, :mileage)";
        $stmt = $pdo->prepare($sql);
        $stmt->execute(array(':make' => $_POST['make'], ':year' => $_POST['year'], 'mileage' => $_POST['mileage']));
        $_SESSION['success'] = "Record inserted";
        header("Location: view.php");
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
            <p>Year: <input type="text" name="year"></p>
            <p>Mileage: <input type="text" name="mileage"></p>
            <input type="submit" name="add" value="Add">
            <input type="submit" name="cancel" value="Cancel">
        </form>
    </div>

</html>