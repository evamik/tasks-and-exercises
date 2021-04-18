{-
Any positive integer can be represented as
sum of squares of other numbers.
Your task is to print the smallest ‘n’ such that N = a_1^2 + a_2^2 + a_3^2 + ... + a_n^s

Sample Input
5
1
2
3
4
50

Sample Output
1
2
3
1
2
-}

getMinSquares n = 
    if n <= 3
        then n
        else loop 1 (n+1) n
    
loop i cond val =
    if i < cond
        then loop (i+1) cond (
            let temp = i*i 
            in if temp > val
                then val 
                else min val (1 + getMinSquares (val-temp))
        )
        else val
        
mainLoop i cond = do
    if i < cond
        then do
            input <- getLine
            let number = read input :: Int
            print (getMinSquares number)
            mainLoop (i+1) cond
        else return ()
    
main = do
    input <- getLine
    if input == "0"
        then return ()
        else do
            let cases = read input :: Int
            mainLoop 0 cases
