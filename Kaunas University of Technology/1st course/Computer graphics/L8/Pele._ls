(Defun C:Pele()
  (setq laidas1 (getpoint "\nNurodykite peles bazini taska:"))
   (setq laidas2 (list(+ 10(car laidas1))(- (cadr laidas1) 6)))
   (setq laidas3 (list(+ 20(car laidas1))(cadr laidas1)))
   (setq laidas4 (list(+ 5(car laidas3))(+ 10(cadr laidas3))))
   (setq laidas5 (list(+ 10(car laidas4))(+ 6(cadr laidas4))))
   (setq laidas6 (list(+ 20(car laidas4))(- (cadr laidas4) 4)))
   (setq taskas1 (list(- (car laidas6) 15)(cadr laidas6)))
   (setq taskas2 (list(+ 8(car laidas6))(cadr laidas6)))
   (setq taskas3 (list(car taskas1)(- (cadr taskas1) 12)))
   (setq taskas4 (list(car laidas6)(cadr taskas3)))
   (setq taskas5 (list(car taskas2)(cadr taskas3)))
   (setq taskas6 (list(car taskas1)(- (cadr taskas1) 30)))
   (setq taskas7 (list(car taskas2)(cadr taskas6)))
   (setq taskas8 (list(+ 5(car taskas6))(- (cadr taskas6) 7)))
   (setq fillet1 (list(car taskas1)(- (cadr taskas1)1)))
   (setq fillet2 (list(+ 1(car taskas1))(cadr taskas1)))
   (setq fillet3 (list(car taskas2)(- (cadr taskas2)1)))
   (setq fillet4 (list(- (car taskas2)1)(cadr taskas2)))
  (command "osnap" "off")
   (command "lweight" "0.7")
   (command "arc" laidas1 laidas2 laidas3)
   (command "line" laidas3 laidas4 "")
   (command "arc" laidas4 laidas5 laidas6)
   (command "lweight" "0.3")
   (command "line" taskas1 taskas2 taskas7 "")
   (command "line" taskas1 taskas3 taskas5 "")
   (command "line" laidas6 taskas4 "")
   (command "line" taskas3 taskas6 "")
   (command "arc" taskas6 taskas8 taskas7)
   (command "fillet" fillet1 "r" "2" fillet2)
   (command "fillet" fillet3 "r" "2" fillet4)
 )