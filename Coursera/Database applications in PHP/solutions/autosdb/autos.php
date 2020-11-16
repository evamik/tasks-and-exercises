<?php
    if(empty($_GET['name'])){
        die("Name parameter missing");
    }
    require_once "pdo.php";

    $failure = false;
    $added = false;

    if(isset($_POST['add']) && isset($_POST['mileage']) && isset($_POST['year']) && isset($_POST['make'])){
        if ($_POST['make'] == ""){
            $failure = "Make is required";
        } else if (!is_numeric($_POST['year'])){
            $failure = "Mileage and year must be numeric";
        } else if (!is_numeric($_POST['mileage'])){
            $failure = "Mileage and year must be numeric";
        } else {
            $sql = "INSERT INTO autos (make, year, mileage) VALUES(:make, :year, :mileage)";
            $stmt = $pdo->prepare($sql);
            $stmt->execute(array(':make' => $_POST['make'], ':year' => $_POST['year'], 'mileage' => $_POST['mileage']));
            $added = true;
        }
    }

    if(isset($_POST['logout'])){
        header('Location:index.php');
    }

    $stmt = $pdo->query("SELECT * FROM autos ORDER BY make");
    $rows = $stmt->fetchAll(PDO::FETCH_ASSOC);
?>
<!DOCTYPE html>
<html>
    <head>
    <title>Evaldas Mikalauskas's Automobile Tracker</title>
    
    <?php require_once "bootstrap.php"; ?>
    
    </head>
    <body>
        <div class="container">
            <h1>Tracking Autos for <?=htmlentities($_GET['name'])?></h1>
            <?php
                echo ('<p style="color: red;">'.htmlentities($failure)."</p>");
                if ($added){
                    echo ('<p style="color: green;">'."Record inserted"."</p>");
                    $added = false;
                }
            ?>
            <form method="post"> 
                <p>Make: <input type="text" name="make" size="60"></p>
                <p>Year: <input type="text" name="year"></p>
                <p>Mileage: <input type="text" name="mileage"></p>
                <input type="submit" name="add" value="Add">
                <input type="submit" name="logout" value="Logout">
            </form>
            <h2>Automobiles</h2>
            <?php
                foreach($rows as $row) {
                    echo "<li>";
                    echo (htmlentities($row['year']));
                    echo " ";
                    echo (htmlentities($row['make']));
                    echo " / ";
                    echo (htmlentities($row['mileage']));
                    echo "</li>";
                }
            ?>
        </div>
</html>
