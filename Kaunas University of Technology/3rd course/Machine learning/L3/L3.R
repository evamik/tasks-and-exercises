library(e1071)
set.seed(1)
x=matrix(rnorm(20*2), ncol=2)
y=c(rep(-1,10), rep(1,10))
x[y==1,]=x[y==1,] + 1
plot(x, col=(3-y))
dat=data.frame(x=x, y=as.factor(y))

svmfit=svm(y~., data=dat, kernel="linear", cost=10,scale=FALSE)
plot(svmfit , dat,grid=100,color.palette = terrain.colors,xlim=c(-2.1,2.5),ylim=c(-1.3,2.6))
svmfit$index
summary(svmfit)

svmfit=svm(y~., data=dat, kernel="linear", cost=0.1, scale=FALSE)
plot(svmfit , dat,grid=100,color.palette = terrain.colors)
svmfit$index

set.seed(1)
tune.out=tune(svm,y~.,data=dat,kernel="linear", ranges=list(cost=c(0.001, 0.01, 0.1, 1,5,10,100)))
summary(tune.out)
bestmod=tune.out$best.model
summary(bestmod)

xtest=matrix(rnorm(20*2), ncol=2)
ytest=sample(c(-1,1), 20, rep=TRUE)
xtest[ytest==1,]=xtest[ytest==1,] + 1
testdat=data.frame(x=xtest, y=as.factor(ytest))
ypred=predict(bestmod ,testdat)
table(predict=ypred, truth=testdat$y)

svmfit=svm(y~., data=dat, kernel="linear", cost=.01, scale=FALSE)
ypred=predict(svmfit ,testdat)
table(predict=ypred, truth=testdat$y)

dat=data.frame(x=x,y=as.factor(y))
svmfit=svm(y~., data=dat, kernel="linear", cost=1e5)
summary(svmfit)
plot(svmfit , dat,grid=100,color.palette = terrain.colors)

svmfit=svm(y~., data=dat, kernel="linear", cost=1)
summary(svmfit)
plot(svmfit ,dat,grid=100,color.palette = terrain.colors)



set.seed(1)
x=matrix(rnorm(200*2), ncol=2)
x[1:100,]=x[1:100,]+2
x[101:150,]=x[101:150,]-2
y=c(rep(1,150),rep(2,50))
dat=data.frame(x=x,y=as.factor(y))
plot(x, col=y)

library(caret)
set.seed(1)
idx.train = createDataPartition(y = dat$y, p = 0.8, list = FALSE)
train = dat[idx.train, ]
test = dat[-idx.train, ]
svmfit=svm(y~., data=train, kernel="radial", gamma=1,cost =1)
plot(svmfit , train)
plot(svmfit , train,grid=100,color.palette = terrain.colors)
summary(svmfit)

svmfit=svm(y~., data=train, kernel="radial", gamma=1,cost=1e5)
plot(svmfit , train,grid=100,color.palette = terrain.colors)

set.seed(1)
tune.out=tune(svm, y~., data=train, kernel="radial", ranges=list(cost=c(0.1,1,10,100,1000),gamma=c(0.5,1,2,3,4) ))
summary(tune.out)
plot(tune.out,color.palette = terrain.colors)
pred=predict(tune.out$best.model, newdata=test)
table(predicted_labels=pred,true_labels=test$y)


set.seed(1)
x=rbind(x, matrix(rnorm(50*2), ncol=2))
y=c(y, rep(0,50))
x[y==0,2]=x[y==0,2]+2
dat=data.frame(x=x, y=as.factor(y))
plot(x,col=(y+1))
svmfit=svm(y~., data=dat, kernel="radial", cost=1, gamma=1)
plot(svmfit , dat,grid=100,color.palette = terrain.colors)


library(ISLR)
names(Khan)
dim(Khan$xtrain )
dim(Khan$xtest )
length(Khan$ytrain )
length(Khan$ytest )
table(Khan$ytrain )
table(Khan$ytest )
dat=data.frame(x=Khan$xtrain , y=as.factor(Khan$ytrain ))
out=svm(y~., data=dat, kernel="linear",cost=10)
summary(out)
table(out$fitted , dat$y)
dat.te=data.frame(x=Khan$xtest , y=as.factor(Khan$ytest ))
pred.te=predict(out, newdata=dat.te)
table(pred.te, dat.te$y)

# SVM
satellite_test <- read.csv("C:/Users/mikal/KTU/Kaunas University of Technology/3rd course/Machine learning/L3/satellite_test.csv")
satellite_train <- read.csv("C:/Users/mikal/KTU/Kaunas University of Technology/3rd course/Machine learning/L3/satellite_train.csv")
satellite_test$V37=as.factor(satellite_test$V37)
satellite_train$V37=as.factor(satellite_train$V37)
levels(satellite_train$V37)=c("red soil", "cotton crop", "grey soil","damp grey soil","soil with vegetation", "very damp grey soil")
levels(satellite_test$V37)=c("red soil", "cotton crop", "grey soil","damp grey soil","soil with vegetation", "very damp grey soil")
#1
plot(satellite_train$V37)

#2
plot(satellite_train$V36, satellite_train$V35, xlab="V36", ylab="V35", col=satellite_train$V37, pch=16)

#3
tune.out=tune(svm, satellite_train$V37~satellite_train$V36+satellite_train$V35, data=satellite_train, kernel="linear", ranges=list(cost=c(0.1,1,10)))
summary(tune.out)
bestmod=tune.out$best.model
summary(bestmod)

dat=data.frame(V36=satellite_train$V36,V35=satellite_train$V35,y=satellite_train$V37)
svmfit=svm(dat$y~.,data=dat,kernel="linear",cost=1,scale=TRUE)
plot(svmfit,dat,grid=100,color.palette=terrain.colors)

test_dat=data.frame(V36=satellite_test$V36,V35=satellite_test$V35,y=satellite_test$V37)
pred=predict(svmfit, test_dat)
table=table(predicted_labels=pred,truth=test_dat$y)
accuracy=diag(table)/colSums((table))
barplot(accuracy)
mean(test_dat$y!=pred)

#4
tune.out=tune(svm, satellite_train$V37~satellite_train$V36+satellite_train$V35, data=satellite_train, kernel="radial", ranges=list(cost=c(0.1,1,10),gamma=c(0.01,0.1,1,2,4) ))
summary(tune.out)

svmfit=svm(dat$y~.,data=dat,kernel="radial",gamma=4, cost=10, scale=TRUE)
plot(svmfit,dat,grid=100,color.palette=terrain.colors)
pred=predict(svmfit, test_dat)
table=table(predicted_labels=pred,truth=test_dat$y)
accuracy=diag(table)/colSums((table))
barplot(accuracy)
mean(test_dat$y!=pred)

#5
# linear
tune.out=tune(svm, V37~., data=satellite_train, kernel="linear", ranges=list(cost=c(0.1,1,10)))
summary(tune.out)
bestmod=tune.out$best.model
summary(bestmod)

train = satellite_train[-37]
dat=data.frame(train,y=satellite_train$V37)
svmfit=svm(dat$y~.,data=dat,kernel="linear",cost=0.1,scale=TRUE)

test_dat=data.frame(satellite_test[-37],y=satellite_test$V37)
pred=predict(svmfit, test_dat)
table=table(predicted_labels=pred,truth=test_dat$y)
accuracy=diag(table)/colSums((table))
barplot(accuracy)
mean(test_dat$y!=pred)

# radial
tune.out=tune(svm, V37~., data=satellite_train, kernel="radial", ranges=list(cost=c(0.1,1,10),gamma=c(0.001,0.1,0.1,1,2) ))
summary(tune.out)
bestmod=tune.out$best.model
summary(bestmod)

svmfit=svm(dat$y~.,data=dat,kernel="radial",gamma=0.1, cost=10, scale=TRUE)
pred=predict(svmfit, test_dat)
table=table(predicted_labels=pred,truth=test_dat$y)
accuracy=diag(table)/colSums((table))
barplot(accuracy)
mean(test_dat$y!=pred)

# Neural networks
library(h2o)
h2o.init(nthreads = -1)

dat=read.csv("C:/Users/mikal/KTU/Kaunas University of Technology/3rd course/Machine learning/L3/customer_churn.csv", stringsAsFactors=TRUE)
head(dat)
str(dat)

plot(density(dat$tenure[dat$Churn=="Yes"]),
     col="firebrick1",lwd=3,main="", xlab="tenure variable, i.e.
number of months the customer is with the company")
lines(density(dat$tenure[dat$Churn=="No"]),
      col="dodgerblue1",lwd=3)
grid()
legend(40, 0.04, legend=c("Churned", "Stayed"),
       col=c("firebrick1", "dodgerblue1"),cex=1.2,lty=1)

plot(density(dat$MonthlyCharges[dat$Churn=="Yes"]),col="red",
     lwd=3,main="",xlab="MonthlyCharges variable",ylim=c(0,0.02))
lines(density(dat$MonthlyCharges[dat$Churn=="No"]),col="blue",lwd=3)
grid()
legend(100, 0.02, legend=c("Stayed", "Churned"),col=c("red",
                                                        "blue"),cex=0.8,lty=1)
dat[,"customerID"]=NULL
library(caret)
idx.train = createDataPartition(y = dat$Churn, p = 0.8, list =
                                  FALSE)
train = dat[idx.train, ]
test = dat[-idx.train, ]

h2o_train=as.h2o(train)
h2o_test=as.h2o(test)


dl_model = h2o.deeplearning(x = names(h2o_train[,1:19]),
                            y = "Churn",
                            training_frame = h2o_train,
                            activation="Tanh",
                            hidden = c(10),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 10000,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0)

plot(dl_model)
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
confMatrix=table(true_labels=test$Churn, predicted_labels=prediction[,1])
confMatrix

diag(confMatrix)/rowSums(confMatrix)*100

dl_model = h2o.deeplearning(x = names(h2o_train[,1:19]),
                            y = "Churn",
                            training_frame = h2o_train,
                            activation="Tanh",
                            hidden =  c(20,20,20),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 10000,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0)
plot(dl_model)
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
confMatrix=table(true_labels=test$Churn, predicted_labels=prediction[,1])
confMatrix
diag(confMatrix)/rowSums(confMatrix)*100

dl_model = h2o.deeplearning(x = names(h2o_train[,1:19]),
                            y = "Churn",
                            training_frame = h2o_train,
                            activation="Tanh",
                            hidden =  c(20,20,20),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 10000,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0,
                            l1=0,
                            l2=0.01)

training_loss1=dl_model@model$scoring_history$training_logloss
plot(training_loss1,type="l",col="olivedrab",lwd=3)
grid()
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
mean(prediction$predict==test$Churn)*100
confMatrix=table(predicted_labels=prediction$predict,
                 true_labels=test$Churn)
diag(confMatrix)/colSums(confMatrix)*100

#1
train=read.csv("C:/Users/mikal/KTU/Kaunas University of Technology/3rd course/Machine learning/L3/arrhythmia_train.csv")
test=read.csv("C:/Users/mikal/KTU/Kaunas University of Technology/3rd course/Machine learning/L3/arrhythmia_test.csv")
train$arrhythmia=as.factor(train$arrhythmia)
test$arrhythmia=as.factor(test$arrhythmia)
levels(train$arrhythmia)=c("normal","supraventricular","ventricular", "unknown")
levels(test$arrhythmia)=c("normal","supraventricular","ventricular", "unknown")

par(mfrow=c(2,5)) 
for (i in c(1:10)){
  y=as.vector(train[i,1:187])
  x=as.vector(c(1:187))
  plot(x,y, type='l')
}
par(mfrow=c(1,1))

#2
plot(train$X1, train$X2, xlab="X1", ylab="X2", col=train$arrhythmia, pch=16)

#3
plot(train$arrhythmia)

#4
h2o_train=as.h2o(train)
h2o_test=as.h2o(test)

# aaaaaaaaaaaaaaaaaaaaaaaa activation="Tanh" aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
dl_model = h2o.deeplearning(x = names(h2o_train[,1:187]),
                            y = "arrhythmia",
                            training_frame = h2o_train,
                            activation="Tanh",
                            hidden =  c(10),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 250,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0)

training_loss1=dl_model@model$scoring_history$training_logloss
plot(training_loss1,type="l",col="olivedrab",lwd=3)
grid()
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
mean(prediction$predict==test$arrhythmia)*100
confMatrix=table(predicted_labels=prediction$predict,
                 true_labels=test$arrhythmia)
temp=confMatrix[3,]
confMatrix[3,]=confMatrix[4,]
confMatrix[4,]=temp
confMatrix
diag(confMatrix)/colSums(confMatrix)*100

# aaaaaaaaaaaaaaaaaaaaaaaa activation="Maxout" aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
dl_model = h2o.deeplearning(x = names(h2o_train[,1:187]),
                            y = "arrhythmia",
                            training_frame = h2o_train,
                            activation="Maxout",
                            hidden =  c(10),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 250,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0)

training_loss1=dl_model@model$scoring_history$training_logloss
plot(training_loss1,type="l",col="olivedrab",lwd=3)
grid()
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
mean(prediction$predict==test$arrhythmia)*100
confMatrix=table(predicted_labels=prediction$predict,
                 true_labels=test$arrhythmia)
temp=confMatrix[3,]
confMatrix[3,]=confMatrix[4,]
confMatrix[4,]=temp
confMatrix
diag(confMatrix)/colSums(confMatrix)*100

# aaaaaaaaaaaaaaaaaaaaaaaa activation="Rectifier" aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
dl_model = h2o.deeplearning(x = names(h2o_train[,1:187]),
                            y = "arrhythmia",
                            training_frame = h2o_train,
                            activation="Rectifier",
                            hidden =  c(10),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 250,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0)

training_loss1=dl_model@model$scoring_history$training_logloss
plot(training_loss1,type="l",col="olivedrab",lwd=3)
grid()
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
mean(prediction$predict==test$arrhythmia)*100
confMatrix=table(predicted_labels=prediction$predict,
                 true_labels=test$arrhythmia)
temp=confMatrix[3,]
confMatrix[3,]=confMatrix[4,]
confMatrix[4,]=temp
confMatrix
diag(confMatrix)/colSums(confMatrix)*100

# aaaaaaaaaaaaaaaaaaaaaaaa hidden =  c(10,10) aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
dl_model = h2o.deeplearning(x = names(h2o_train[,1:187]),
                            y = "arrhythmia",
                            training_frame = h2o_train,
                            activation="Maxout",
                            hidden =  c(10,10),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 250,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0)

training_loss1=dl_model@model$scoring_history$training_logloss
plot(training_loss1,type="l",col="olivedrab",lwd=3)
grid()
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
mean(prediction$predict==test$arrhythmia)*100
confMatrix=table(predicted_labels=prediction$predict,
                 true_labels=test$arrhythmia)
temp=confMatrix[3,]
confMatrix[3,]=confMatrix[4,]
confMatrix[4,]=temp
confMatrix
diag(confMatrix)/colSums(confMatrix)*100


# aaaaaaaaaaaaaaaaaaaaaaaa hidden =  c(20,20) aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
dl_model = h2o.deeplearning(x = names(h2o_train[,1:187]),
                            y = "arrhythmia",
                            training_frame = h2o_train,
                            activation="Maxout",
                            hidden =  c(20,20),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 250,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0)

training_loss1=dl_model@model$scoring_history$training_logloss
plot(training_loss1,type="l",col="olivedrab",lwd=3)
grid()
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
mean(prediction$predict==test$arrhythmia)*100
confMatrix=table(predicted_labels=prediction$predict,
                 true_labels=test$arrhythmia)
temp=confMatrix[3,]
confMatrix[3,]=confMatrix[4,]
confMatrix[4,]=temp
confMatrix
diag(confMatrix)/colSums(confMatrix)*100


# aaaaaaaaaaaaaaaaaaaaaaaa l1=0,l2=0.005 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
dl_model = h2o.deeplearning(x = names(h2o_train[,1:187]),
                            y = "arrhythmia",
                            training_frame = h2o_train,
                            activation="Maxout",
                            hidden =  c(20,20),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 250,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0,
                            l1=0,
                            l2=0.005)

training_loss1=dl_model@model$scoring_history$training_logloss
plot(training_loss1,type="l",col="olivedrab",lwd=3)
grid()
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
mean(prediction$predict==test$arrhythmia)*100
confMatrix=table(predicted_labels=prediction$predict,
                 true_labels=test$arrhythmia)
temp=confMatrix[3,]
confMatrix[3,]=confMatrix[4,]
confMatrix[4,]=temp
confMatrix
diag(confMatrix)/colSums(confMatrix)*100

# aaaaaaaaaaaaaaaaaaaaaaaa l1=0,l2=0.001 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
dl_model = h2o.deeplearning(x = names(h2o_train[,1:187]),
                            y = "arrhythmia",
                            training_frame = h2o_train,
                            activation="Maxout",
                            hidden =  c(20,20),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 250,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0,
                            l1=0,
                            l2=0.001)

training_loss1=dl_model@model$scoring_history$training_logloss
plot(training_loss1,type="l",col="olivedrab",lwd=3)
grid()
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
mean(prediction$predict==test$arrhythmia)*100
confMatrix=table(predicted_labels=prediction$predict,
                 true_labels=test$arrhythmia)
temp=confMatrix[3,]
confMatrix[3,]=confMatrix[4,]
confMatrix[4,]=temp
confMatrix
diag(confMatrix)/colSums(confMatrix)*100

# aaaaaaaaaaaaaaaaaaaaaaaa l1=0,l2=0.0005 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
dl_model = h2o.deeplearning(x = names(h2o_train[,1:187]),
                            y = "arrhythmia",
                            training_frame = h2o_train,
                            activation="Maxout",
                            hidden =  c(20,20),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 250,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0,
                            l1=0,
                            l2=0.0005)

training_loss1=dl_model@model$scoring_history$training_logloss
plot(training_loss1,type="l",col="olivedrab",lwd=3)
grid()
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
mean(prediction$predict==test$arrhythmia)*100
confMatrix=table(predicted_labels=prediction$predict,
                 true_labels=test$arrhythmia)
temp=confMatrix[3,]
confMatrix[3,]=confMatrix[4,]
confMatrix[4,]=temp
confMatrix
diag(confMatrix)/colSums(confMatrix)*100

# aaaaaaaaaaaaaaaaaaaaaaaa epochs = 100 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
dl_model = h2o.deeplearning(x = names(h2o_train[,1:187]),
                            y = "arrhythmia",
                            training_frame = h2o_train,
                            activation="Maxout",
                            hidden =  c(20,20),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 100,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0,
                            l1=0,
                            l2=0.001)

training_loss1=dl_model@model$scoring_history$training_logloss
plot(training_loss1,type="l",col="olivedrab",lwd=3)
grid()
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
mean(prediction$predict==test$arrhythmia)*100
confMatrix=table(predicted_labels=prediction$predict,
                 true_labels=test$arrhythmia)
temp=confMatrix[3,]
confMatrix[3,]=confMatrix[4,]
confMatrix[4,]=temp
confMatrix
diag(confMatrix)/colSums(confMatrix)*100

# aaaaaaaaaaaaaaaaaaaaaaaa epochs = 500 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
dl_model = h2o.deeplearning(x = names(h2o_train[,1:187]),
                            y = "arrhythmia",
                            training_frame = h2o_train,
                            activation="Maxout",
                            hidden =  c(20,20),
                            loss = "CrossEntropy",
                            score_each_iteration=TRUE,
                            epochs = 500,
                            rate=0.01,
                            balance_classes=F,
                            adaptive_rate = F,
                            stopping_rounds=0,
                            l1=0,
                            l2=0.001)

training_loss1=dl_model@model$scoring_history$training_logloss
plot(training_loss1,type="l",col="olivedrab",lwd=3)
grid()
prediction = as.data.frame(h2o.predict(object = dl_model,
                                       newdata = h2o_test))
mean(prediction$predict==test$arrhythmia)*100
confMatrix=table(predicted_labels=prediction$predict,
                 true_labels=test$arrhythmia)
temp=confMatrix[3,]
confMatrix[3,]=confMatrix[4,]
confMatrix[4,]=temp
confMatrix
diag(confMatrix)/colSums(confMatrix)*100
