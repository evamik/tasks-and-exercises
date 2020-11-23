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
$autos_id = $row['autos_id'];

if (isset($_POST['delete']) && isset($_POST['autos_id'])) {
    $stmt = $pdo->prepare("DELETE FROM autos WHERE autos_id = :autos_id");
    $stmt->execute(array(":autos_id" => $_POST['autos_id']));
    $_SESSION['success'] = "Record deleted";
    header("Location: index.php");
    return;
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
        <p>Confirm: Deleting <?= $make ?></p>
        <form method="post">
            <input type="hidden" name="autos_id" value="<?= $autos_id ?>">
            <input type="submit" name="delete" value="Delete">
            <a href="index.php">Cancel</a>
        </form>
    </div>

</html>