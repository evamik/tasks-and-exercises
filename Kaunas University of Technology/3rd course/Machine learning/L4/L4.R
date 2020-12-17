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


#  Smartphone-Based Recognition of Human Activities and Postural Transitions
train <- read.csv(paste(dir, "activity_train.csv", sep="/"))
test <- read.csv(paste(dir, "activity_test.csv", sep="/"))
train$activity=as.factor(train$activity)
test$activity=as.factor(test$activity)


#1
plot(train$tBodyAcc_Mean_1, train$tBodyAcc_STD_1, col=(train$activity), pch=16)
plot(train$tBodyAcc_Mean_2, train$tBodyAcc_STD_2, col=(train$activity), pch=16)
plot(train$tBodyAcc_Mean_3, train$tBodyAcc_STD_3, col=(train$activity), pch=16)

mtries <- numeric(15)
ntrees <- numeric(15)
means <- numeric(15)
#2
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
train_label
bst = xgboost(data = as.matrix(train[-1]),
              label = train_label,
              num_class=12,
              max_depth = 1,
              nrounds = 100,
              objective = "multi:softmax")
