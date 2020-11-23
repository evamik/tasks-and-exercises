<?php
require_once "pdo.php";
session_start();
if (!isset($_SESSION['name'])) {
    die("ACCESS DENIED");
}

if (isset($_POST['cancel'])) {
    header("Location: index.php");
    return;
}

$stmt = $pdo->prepare("SELECT * FROM autos WHERE autos_id = :xyz");
$stmt->execute(array(":xyz" => $_GET['autos_id']));
$row = $stmt->fetch(PDO::FETCH_ASSOC);

if ($row === false) {
    $_SESSION['failure'] = 'Bad value for id';
    header('Location: index.php');
    return;
}

$make = htmlentities($row['make']);
$model = htmlentities($row['model']);
$mileage = htmlentities($row['mileage']);
$year = htmlentities($row['year']);
$autos_id = $row['autos_id'];

if (isset($_POST['save']) && isset($_POST['mileage']) && isset($_POST['year']) && isset($_POST['make']) && isset($_POST['model'])) {
    if ($_POST['make'] == "" || $_POST['mileage'] == "" || $_POST['year'] == "" || $_POST['model'] == "") {
        $_SESSION['failure'] = "All fields are required";
        header("Location: edit.php");
        return;
    } else if (!is_numeric($_POST['year']) && !is_numeric($_POST['mileage'])) {
        $_SESSION['failure'] = "Mileage and year must be numeric";
        header("Location: edit.php");
        return;
    } else if (!is_numeric($_POST['year'])) {
        $_SESSION['failure'] = "Year must be numeric";
        header("Location: edit.php");
        return;
    } else if (!is_numeric($_POST['mileage'])) {
        $_SESSION['failure'] = "Mileage must be numeric";
        header("Location: edit.php");
        return;
    } else {
        $sql = "UPDATE autos SET make = :make, 
                year = :year, mileage = :mileage, model = :model 
                WHERE autos_id = :autos_id";
        $stmt = $pdo->prepare($sql);
        $stmt->execute(array(
            ':make' => $_POST['make'],
            ':year' => $_POST['year'],
            'mileage' => $_POST['mileage'],
            'model' => $_POST['model'],
            'autos_id' => $_POST['autos_id']
        ));
        $_SESSION['success'] = "Record edited";
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
            <p>Make: <input type="text" name="make" size="60" value="<?= $make ?>"></p>
            <p>Model: <input type="text" name="model" size="60" value="<?= $model ?>"></p>
            <p>Year: <input type="text" name="year" value="<?= $year ?>"></p>
            <p>Mileage: <input type="text" name="mileage" value="<?= $mileage ?>"></p>
            <input type="hidden" name="autos_id" value="<?= $autos_id ?>">
            <input type="submit" name="save" value="Save">
            <input type="submit" name="cancel" value="Cancel">
        </form>
    </div>

</html>