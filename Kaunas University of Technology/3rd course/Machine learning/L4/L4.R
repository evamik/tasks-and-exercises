library(caret)
library(randomForest)
library(xgboost)
dir <- dirname(rstudioapi::getSourceEditorContext()$path)

data <- read.csv(paste(dir, "breastcancer.csv", sep="/"), stringsAsFactors=TRUE)
idx.train = createDataPartition(y = data$diagnosis, p = 0.8,
                                list = FALSE)
train = data[idx.train, ]
test = data[-idx.train, ]
rf=randomForest(diagnosis~.,data=train,ntree=1000,importance=T)
plot(rf)
varImpPlot(rf)
pred=predict(rf,test)
mean(pred==test$diagnosis)*100
confMat=table(true_lab=test$diagnosis, predicted=pred)
diag(confMat)/rowSums(confMat)
train_label=as.numeric(train$diagnosis)-1
test_label=as.numeric(test$diagnosis)-1
bst = xgboost(data = as.matrix(train[,2:31]), label = train_label, num_class=2, max_depth=1, nrounds = 100, objective = "multi:softmax")

pred = predict(bst, as.matrix(test[,2:31]))
mean(pred==test_label)*100
confMat=table(true_lab=test_label, predicted=pred)
diag(confMat)/rowSums(confMat)


#  Random forests, boosting
train <- read.csv(paste(dir, "activity_train.csv", sep="/"))
test <- read.csv(paste(dir, "activity_test.csv", sep="/"))
train$activity=as.factor(train$activity)
test$activity=as.factor(test$activity)


#1
plot(train$tBodyAcc_Mean_1, train$tBodyAcc_STD_1, col=(train$activity), pch=16)
plot(train$tBodyAcc_Mean_2, train$tBodyAcc_STD_2, col=(train$activity), pch=16)
plot(train$tBodyAcc_Mean_3, train$tBodyAcc_STD_3, col=(train$activity), pch=16)


#2
mtries <- numeric(15)
ntrees <- numeric(15)
means <- numeric(15)
for(i in 1:5) {
  for(j in 1:3) {
    rf=randomForest(activity~.,data=train,mtry=2^(i-1),ntree=25*j,importance=T)
    pred=predict(rf,test)
    ind = (i-1)*3+(j)
    means[ind] = mean(pred==test$activity)*100
    mtries[ind] = 2^(i-1)
    ntrees[ind] = j*25
  }
}
results <- data.frame(mtry=mtries, ntree=ntrees, mean=means)
results
results$mtry = as.factor(results$mtry)
ggplot(data = results, aes(x = ntree, y = mean, color = mtry)) + geom_line()

#3
train_label=as.numeric(train$activity)-1
test_label=as.numeric(test$activity)-1


max_depths <- numeric(25)
nrounds <- numeric(25)
means <- numeric(25)
for(i in 1:5) {
  for(j in 1:5) {
    ind = (i-1)*5+(j)
    bst = xgboost(data = as.matrix(train[-562]),
                  label = train_label,
                  num_class=12,
                  max_depth = j,
                  nrounds = 20*i,
                  objective = "multi:softmax")
    pred = predict(bst, as.matrix(test[-562]))
    means[ind] = mean(pred==test_label)*100
    max_depths[ind] = j
    nrounds[ind] = 20*i
  }
}
results2 <- data.frame(max_depth=max_depths, nrounds=nrounds, mean=means)
results2
results2$max_depth=as.factor(results2$max_depth)
ggplot(data = results2, aes(x = nrounds, y = mean, color = max_depth)) + geom_line()

max_depths <- numeric(15)
nrounds <- numeric(15)
means <- numeric(15)
for(i in 1:3) {
  for(j in 1:5) {
    ind = (i-1)*5+(j)
    bst = xgboost(data = as.matrix(train[-562]),
                  label = train_label,
                  num_class=12,
                  max_depth = j,
                  nrounds = 200*i,
                  objective = "multi:softmax")
    pred = predict(bst, as.matrix(test[-562]))
    means[ind] = mean(pred==test_label)*100
    max_depths[ind] = j
    nrounds[ind] = 100+100*i
  }
}
results3 <- data.frame(max_depth=max_depths, nrounds=nrounds, mean=means)
results3
results3$max_depth=as.factor(results3$max_depth)
ggplot(data = results3, aes(x = nrounds, y = mean, color = max_depth)) + geom_line()


# PCA, clustering

data <- read.csv(paste(dir, "customer_churn.csv", sep="/"), stringsAsFactors=TRUE)
set.seed(1)
idx.train = createDataPartition(y = data$Churn, p = 0.8, list =
                                  FALSE)
train = data[idx.train, ]
test = data[-idx.train, ]

train_numeric=train
test_numeric=test
for(i in 1:19){
  if(is.factor(train_numeric[,i])){
    train_numeric[,i]=as.numeric(train_numeric[,i])
    test_numeric[,i]=as.numeric(test_numeric[,i])
  }
}

pr.out=prcomp(train_numeric[,1:19], scale=TRUE)

par(mfrow=c(1,2))
plot(pr.out$x[,1:2], col=c("blue", "red")[train$Churn],
     pch=16,xlab="Z1",ylab="Z2")
plot(pr.out$x[,c(2,3)], col=c("blue", "red")[train$Churn],
     pch=16,xlab="Z1",ylab="Z3")

par(mfrow=c(1,2))
plot(pr.out$x[,1:2], col=c("blue", "red")[train$PhoneService],
     pch=16,xlab="Z1",ylab="Z2")
plot(pr.out$x[,c(2,3)], col=c("blue", "red")[train$PhoneService],
     pch=16,xlab="Z1",ylab="Z3")

par(mfrow=c(1,2))
plot(summary(pr.out)$importance[2,],lwd=3,col="red",type="l",
     ylab="Explained variance by a PC",xlab="Number of PCs")
grid()
plot(summary(pr.out)$importance[3,],lwd=3,col="red",type="l",
     ylab="Cumulative fraction of explained variance",xlab="Number
of PCs")
grid()


train_z=pr.out$x[,1:12]
prd=predict(pr.out,test_numeric)
test_z=prd[,1:12]
train_z=data.frame(train_z,Churn=train$Churn)
test_z=data.frame(test_z,Churn=test$Churn)

rf=randomForest(Churn~.,data=train_z, ntree=1000,importance=T)
pred=predict(rf,test_z)
mean(pred==test_z$Churn)*100
confMat=table(true_lab=test_z$Churn,predicted=pred)
diag(confMat)/rowSums(confMat)

rf=randomForest(Churn~.,data=train, ntree=1000,importance=T)
pred=predict(rf,test)
mean(pred==test$Churn)*100
confMat=table(true_lab=test$Churn,predicted=pred)
diag(confMat)/rowSums(confMat)

#1
train <- read.csv(paste(dir, "activity_train.csv", sep="/"))
test <- read.csv(paste(dir, "activity_test.csv", sep="/"))
train$activity=as.factor(train$activity)
test$activity=as.factor(test$activity)

train_numeric=train
test_numeric=test
for(i in 1:561){
  if(is.factor(train_numeric[,i])){
    train_numeric[,i]=as.numeric(train_numeric[,i])
    test_numeric[,i]=as.numeric(test_numeric[,i])
  }
}

pr.out=prcomp(train_numeric[,1:561], scale=TRUE)
par(mfrow=c(1,1))
plot(summary(pr.out)$importance[3,],lwd=3,col="red",type="l",
     ylab="Cumulative fraction of explained variance",xlab="Number
of PCs", las=1, xaxt="none")
axis(side=2, c(0.99,0.95), las=1)
axis(side=1, seq(0,561,25))
grid(25,56)

#2
train_z=pr.out$x[,1:105]
prd=predict(pr.out,test_numeric)
test_z=prd[,1:105]
train_z=data.frame(train_z,activity=train$activity)
test_z=data.frame(test_z,activity=test$activity)

mtries <- numeric(15)
ntrees <- numeric(15)
means <- numeric(15)
for(i in 1:5) {
  for(j in 1:3) {
    rf=randomForest(activity~.,data=train_z,mtry=2^(i-1),ntree=25*j,importance=T)
    pred=predict(rf,test_z)
    ind = (i-1)*3+(j)
    means[ind] = mean(pred==test_z$activity)*100
    mtries[ind] = 2^(i-1)
    ntrees[ind] = j*25
  }
}
results <- data.frame(mtry=mtries, ntree=ntrees, mean=means)
results
results$mtry = as.factor(results$mtry)
ggplot(data = results, aes(x = ntree, y = mean, color = mtry)) + geom_line()
