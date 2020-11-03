data = read.csv("~/HTRU_2.csv", header=FALSE)
data = HTRU_2
data$V9=as.factor(data$V9)
plot(data$V9)
library(caret)
library(MASS)
idx.train=createDataPartition(y = data$V9, p = 0.8, list = FALSE)
train = data[idx.train, ]
test = data[-idx.train, ]

lda_classifier = lda(formula = V9 ~ ., data = train)
predictions=predict(lda_classifier,test)$class
table(predicted=predictions,true_labels=test$V9)

qda_classifier = qda(formula = V9 ~ ., data = train)
predictions = predict(qda_classifier, test)$class
table(predicted=predictions,true_labels=test$V9)

cols = c("#808000", "#800080")
plot(train$V2, train$V6, col=cols[train$V9])
grid()


#Exercises with R
#1.
train=sign_mnist_train
test=sign_mnist_test
par(mfrow=c(4,4), tcl = -0.5, mai = c(0.1,0.1,0.1,0.1), xaxt='n',yaxt='n')
id_letter=which(train$label==4)
for(i in id_letter[1:16]){
  hand_sign = as.matrix(train[i,2:785])
  image(matrix(hand_sign, 28,28, byrow=F), col=gray.colors(255))
}

#2.
par(mfrow=c(1,1))
lda_classifier = lda(formula = label ~ ., data = train)
predictions=predict(lda_classifier, test)$class
table = table(predicted=predictions, true_labels=test$label)
mean(test$label==predictions)
accuracies = diag(table)/colSums(table)
names = names(accuracies)
values = as.vector(accuracies)
data = data.frame(names, values)
data$names <- factor(data$names, levels= data$names)
ggplot(data=data, aes(x=names, y=values)) + geom_bar(stat="identity") + xlab("Letters") + ylab("Accuracy")

#3.
train2 = train[,c(1,seq(2,785,11))]
lda_classifier = lda(formula = label ~ ., data = train2)
predictions=predict(lda_classifier, test)$class
table = table(predicted=predictions, true_labels=test$label)
mean(test$label==predictions)
accuracies = diag(table)/colSums(table)
names = names(accuracies)
values = as.vector(accuracies)
data = data.frame(names, values)
data$names <- factor(data$names, levels= data$names)
ggplot(data=data, aes(x=names, y=values)) + geom_bar(stat="identity") + xlab("Letters") + ylab("Accuracy")

#----
means <- 20
for(i in 11:20){
  train2 = train[,c(1,seq(2,785,i))]
  lda_classifier = lda(formula = label ~ ., data = train2)
  predictions=predict(lda_classifier, test)$class
  table = table(predicted=predictions, true_labels=test$label)
  means[i] <- mean(test$label==predictions)}
means
#----

train2 = train[,c(1,seq(2,785,5))]
lda_classifier = lda(formula = label ~ ., data = train2)
predictions=predict(lda_classifier, test)$class
table = table(predicted=predictions, true_labels=test$label)
mean(test$label==predictions)
accuracies = diag(table)/colSums(table)
names = names(accuracies)
values = as.vector(accuracies)
data = data.frame(names, values)
data$names <- factor(data$names, levels= data$names)
ggplot(data=data, aes(x=names, y=values)) + geom_bar(stat="identity") + xlab("Letters") + ylab("Accuracy")

#4.
qda_classifier = qda(formula = label ~ ., data = train)
predictions=predict(qda_classifier, test)$class
table = table(predicted=predictions, true_labels=test$label)
mean(test$label==predictions)
accuracies = diag(table)/colSums(table)
names = names(accuracies)
values = as.vector(accuracies)
data = data.frame(names, values)
data$names <- factor(data$names, levels= data$names)
ggplot(data=data, aes(x=names, y=values)) + geom_bar(stat="identity") + xlab("Letters") + ylab("Accuracy")

#5.
train2 = train[,c(1,seq(2,785,3))]
qda_classifier = qda(formula = label ~ ., data = train2)
predictions=predict(qda_classifier, test)$class
table = table(predicted=predictions, true_labels=test$label)
mean(test$label==predictions)
accuracies = diag(table)/colSums(table)
names = names(accuracies)
values = as.vector(accuracies)
data = data.frame(names, values)
data$names <- factor(data$names, levels= data$names)
ggplot(data=data, aes(x=names, y=values)) + geom_bar(stat="identity") + xlab("Letters") + ylab("Accuracy")

means <- 20
for(i in 1:20){
  train2 = train[,c(1,seq(2,785,i))]
  qda_classifier = qda(formula = label ~ ., data = train2)
  predictions=predict(qda_classifier, test)$class
  table = table(predicted=predictions, true_labels=test$label)
  means[i] <- mean(test$label==predictions)}
means
#----

library(rpart)
library(ISLR)
install.packages("rpart.plot")
library(rpart.plot)
attach(Carseats)
High=ifelse(Sales<=8,"No","Yes")
Carseats=data.frame(Carseats, High)
tree.carseats = rpart(High~.-Sales ,Carseats, method = "class", usesurrogate = 0, cp = 0)
cfit = tree.carseats
printcp(cfit)
plotcp(cfit)
summary(cfit)
plotcp(cfit)

set.seed(1)
idx.train = createDataPartition(y = Boston$medv, p = 0.8,list = FALSE)
train=Boston[idx.train,]
test=Boston[-idx.train,]
cfit=rpart(medv~.,train,method="anova",control=rpart.control(cp=0))
print(cfit)
plotcp(cfit)

cfit_pruned=prune(cfit,cp=0.00281127)
yhat_max=predict(cfit,newdata = test)
yhat_pruned=predict(cfit_pruned,newdata = test)
plot(yhat_max,test$medv,col="blue")
points(yhat_pruned,test$medv,col="red")
c(cor(yhat_max,test$medv)^2, cor(yhat_pruned, test$medv)^2)
rpart.plot(cfit_pruned,fallen.leaves=FALSE,tweak=1.4)

#1
set.seed(2)
dev.off()
music = music_spotify
music = music[-1]
music = music[-15]
music = music[-15]
#a)
idx.train = createDataPartition(y = music$target, p = 0.8,list = FALSE)
train=music[idx.train,]
test=music[-idx.train,]
cfit=rpart(target~.,data=train,method="class",control=rpart.control(cp=0))
printcp(cfit)
plotcp(cfit)
#b)
cfit_pruned=prune(cfit,cp=0.0112641)
rpart.plot(cfit_pruned, fallen.leaves=FALSE, tweak=1.4, extra=101, type=1, box.palette = "YlGnBl")
#c)
sum(cfit_pruned$frame$var == "<leaf>")
printcp(cfit_pruned)
#e)
#unpruned test
yhat_max=predict(cfit,newdata = test,type="class")
table = table(predicted=yhat_max, true_labels=test$target)
mean(test$target==yhat_max)
diag(table)/colSums(table)

#pruned test
yhat_pruned=predict(cfit_pruned,newdata = test,type="class")
table = table(predicted=yhat_pruned, true_labels=test$target)
mean(test$target==yhat_pruned)
diag(table)/colSums(table)

#g) 
names = names(cfit_pruned$variable.importance)
values = cfit_pruned$variable.importance
data = data.frame(names, values)
data$names <- factor(data$names, levels= data$names)
ggplot(data = data, aes(x=names, y=values)) + geom_bar(stat="identity") + xlab("Features") + ylab("Importance")


#2 ---------------------------------------
#a)
train=sign_mnist_train
test=sign_mnist_test
set.seed(1)
cfit=rpart(label~.,data=train,method="class",control=rpart.control(cp=0))
printcp(cfit)
plotcp(cfit)
cfit_pruned=prune(cfit,cp=0)
rpart.plot(cfit_pruned, fallen.leaves=FALSE, tweak=1.4, extra=101, type=1, box.palette = "YlGnBl")
#b)
sum(cfit_pruned$frame$var == "<leaf>")
printcp(cfit_pruned)
#c)
yhat_max=predict(cfit,newdata = test,type="class")
table = table(predicted=yhat_max, true_labels=test$label)
mean(test$label==yhat_max)
diag(table)/colSums(table)
accuracies = diag(table)/colSums(table)
names = names(accuracies)
values = as.vector(accuracies)
data = data.frame(names, values)
data$names <- factor(data$names, levels= data$names)
ggplot(data=data, aes(x=names, y=values)) + geom_bar(stat="identity") + xlab("Letters") + ylab("Accuracy")
#d)
train2 = train[,c(1,seq(2,785,20))]
set.seed(1)
cfit=rpart(label~.,data=train2,method="class",control=rpart.control(cp=0))
printcp(cfit)
plotcp(cfit)
yhat_max=predict(cfit,newdata = test,type="class")
table = table(predicted=yhat_max, true_labels=test$label)
mean(test$label==yhat_max)
diag(table)/colSums(table)

train_var1$class = as.factor(train_var1$class)
test_var1$class = as.factor(test_var1$class)

train = train_var1[-1]
test = test_var1[-1]
lda_classifier = lda(formula = class ~ ., data = train)
predictions=predict(lda_classifier, test)$class
table = table(predicted=predictions, true_labels=test$class)
mean(test$class==predictions)
diag(table)/colSums(table)

train = train_var1
test = test_var1
qda_classifier = qda(formula = class ~ ., data = train)
predictions=predict(qda_classifier, test)$class
table = table(predicted=predictions, true_labels=test$class)
mean(test$class==predictions)
diag(table)/colSums(table)


train = train_var1[-3]
test = test_var1[-3]
lda_classifier = lda(formula = class ~ ., data = train)
predictions=predict(lda_classifier, test)$class
table = table(predicted=predictions, true_labels=test$class)
mean(test$class==predictions)
diag(table)/colSums(table)

train = train_var1
test = test_var1
cfit = rpart(class~. ,train,control=rpart.control(cp=0,minbucket=0,minsplit=0))
plotcp(cfit)
printcp(cfit)
cfit_pruned=prune(cfit,cp=0.0045)
printcp(cfit_pruned)
yhat_max=predict(cfit,newdata = test,type="class")
table = table(predicted=yhat_max, true_labels=test$class)
mean(test$class==yhat_max)
yhat_max=predict(cfit_pruned,newdata = test,type="class")
table = table(predicted=yhat_max, true_labels=test$class)
mean(test$class==yhat_max)

cfit = rpart(class~. ,train,control=rpart.control(cp=0,minbucket=0,minsplit=0))
yhat_max=predict(cfit,newdata = test,type="class")
table = table(predicted=yhat_max, true_labels=test$class)
mean(test$class==yhat_max)

cfit = rpart(class~. ,test,control=rpart.control(cp=0,minbucket=0,minsplit=0))
yhat_max=predict(cfit_pruned,newdata = train,type="class")
table = table(predicted=yhat_max, true_labels=train$class)
mean(train$class==yhat_max)
