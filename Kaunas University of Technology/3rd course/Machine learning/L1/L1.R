library(MASS)
library(ISLR)
library(caret) # This library has, in addition to other fun things, necessary splitting function
library(ggplot2) # ggplot2 library has many tricks for nice plotting
library(cowplot) # in this library there is a plot grid function, among others music$target=as.factor(music$target) # Here we change the type of target from numerical to factor.

data(Boston)
names(Boston)

lm.fit=lm(medv~lstat,data=Boston)
attach(Boston)
plot(lstat ,medv)
abline(lm.fit)

abline(lm.fit,lwd=3)
abline(lm.fit,lwd=3,col="red")
plot(lstat,medv,col="red")
plot(lstat,medv,pch=20)
plot(lstat,medv,pch="+")
plot(1:20,1:20,pch=1:20)

plot(predict(lm.fit),Boston$medv,xlab="Predicted medv
values", ylab="True medv values",ylim=c(0,50))
abline(0,1,col="red",lwd=3)
grid()

lm.fit=lm(medv~lstat+age,data=Boston)
summary(lm.fit)

lm.fit=lm(medv~.,data=Boston)
summary(lm.fit)

plot(predict(lm.fit),Boston$medv,xlab="Predicted medv
values", ylab="True medv values",ylim=c(0,50))
abline(0,1,col="red",lwd=3)
grid()

lm.fit1=lm(medv~.-age,data=Boston)
summary(lm.fit1)

summary(lm(medv~lstat*age,data=Boston))

lm.fit2=lm(medv~lstat+I(lstat^2))
summary(lm.fit2)

data(Carseats)
names(Carseats)

set.seed(1) # this line is necessary in order to always have the same split
idx.train = createDataPartition(y = Carseats$ Sales, p = 0.8,
                                  list = FALSE)
train = Carseats[idx.train, ] # training set with 80 % of Auto data
# test set with p = 0.2 (drop all observations with train indices)
test = Carseats[-idx.train, ]
lm.fit=lm(Sales~.+Income:Advertising+Price:Age,data=train)
summary(lm.fit)

attach(Carseats)
contrasts(ShelveLoc)

pred=predict(lm.fit,test)
plot(pred,test$Sales,xlab="Predicted Sales values",
       ylab="True Sales values",pch=16)
abline(0,1,lwd=3,col="red")
grid()
Rsquared=cor(pred,test$Sales)^2 # R squared measure of fit for testing dataset

#L1.1 *******************************************************
data(Auto)

#1 -----------------------------------------------
str(Auto)
# 392 observations. 9 variables. 
# num, Factor
# ------------------------------------------------

#2 -----------------------------------------------
plot(Auto$year, Auto$mpg)
# From plot we can see newer cars can drive more miles per gallon
# ------------------------------------------------

#3 -----------------------------------------------
pairs(Auto)
# displacement, weight and horsepower have negative effect on mpg.
# ------------------------------------------------

#4 -----------------------------------------------
cor(Auto[,-9])
# ------------------------------------------------

#5 -----------------------------------------------
idx.train=createDataPartition(y=Auto$mpg,p=0.8,list=FALSE)
train=Auto[idx.train,]
test=Auto[-idx.train,]
lm.fit=lm(mpg~., data=train[,-9])
pred=predict(lm.fit,test[,-9])
summary(lm.fit)
str(test$mpg)
plot(pred,test$mpg,xlab="Predicted mpg values", ylab="True mpg values",pch=16)
# ------------------------------------------------

#6 -----------------------------------------------
idx.train=createDataPartition(y=Auto$mpg,p=0.8,list=FALSE)
train=Auto[idx.train,]
test=Auto[-idx.train,]
lm.fit=lm(mpg~horsepower*year, data=train[,-9])
summary(lm.fit)
pred=predict(lm.fit,test)
plot(pred,test$mpg,xlab="Predicted mpg values", ylab="True mpg values",pch=16)
abline(0,1,lwd=3,col="red")
grid()

idx.train=createDataPartition(y=Auto$mpg,p=0.8,list=FALSE)
train=Auto[idx.train,]
test=Auto[-idx.train,]
lm.fit=lm(mpg~horsepower*acceleration, data=train[,-9])
summary(lm.fit)
pred=predict(lm.fit,test)
plot(pred,test$mpg,xlab="Predicted mpg values", ylab="True mpg values",pch=16)
abline(0,1,lwd=3,col="red")
grid()
# a lot of interactions appear to be useful since their R-squared is >0.5

#7 -----------------------------------------------
idx.train=createDataPartition(y=Auto$mpg,p=0.8,list=FALSE)
train=Auto[idx.train,]
test=Auto[-idx.train,]
lm.fit=lm(mpg~weight, data=train[,-9])
summary(lm.fit)
pred=predict(lm.fit,test)
plot(pred,test$mpg,xlab="Predicted mpg values", ylab="True mpg values",pch=16)
abline(0,1,lwd=3,col="red")
grid()

idx.train=createDataPartition(y=Auto$mpg,p=0.8,list=FALSE)
train=Auto[idx.train,]
test=Auto[-idx.train,]
lm.fit=lm(mpg~weight+log(weight), data=train[,-9])
summary(lm.fit)
pred=predict(lm.fit,test)
plot(pred,test$mpg,xlab="Predicted mpg values", ylab="True mpg values",pch=16)
abline(0,1,lwd=3,col="red")
grid()

idx.train=createDataPartition(y=Auto$mpg,p=0.8,list=FALSE)
train=Auto[idx.train,]
test=Auto[-idx.train,]
lm.fit=lm(mpg~weight+I(weight^2), data=train[,-9])
summary(lm.fit)
pred=predict(lm.fit,test)
plot(pred,test$mpg,xlab="Predicted mpg values", ylab="True mpg values",pch=16)
abline(0,1,lwd=3,col="red")
grid()

idx.train=createDataPartition(y=Auto$mpg,p=0.8,list=FALSE)
train=Auto[idx.train,]
test=Auto[-idx.train,]
lm.fit=lm(mpg~weight+sqrt(weight), data=train[,-9])
summary(lm.fit)
pred=predict(lm.fit,test)
plot(pred,test$mpg,xlab="Predicted mpg values", ylab="True mpg values",pch=16)
abline(0,1,lwd=3,col="red")
grid()
#Transformations of the variables seem to yield a higher R-squared value.

#L1.2 ******************************************************
#1 -----------------------------------------------
music = music_spotify
music <- read_csv("C:/Users/mikal/OneDrive - Kaunas University of Technology/3 Kursas/Machine Learning/L1/music_spotify.csv")
str(music_spotify)
head(music, 5)
# --??Describe its structure, produce several examples of its entries.??--

#2 -----------------------------------------------
music$target=as.factor(music$target)
p1 <- ggplot(data=music,aes(x=acousticness,fill=target)) + geom_density() + labs(title="Acousticness by target")
p2 <- ggplot(data=music,aes(x=danceability,fill=target)) + geom_density() + labs(title="Danceability by target")
p3 <- ggplot(data=music,aes(x=duration_ms,fill=target)) + geom_density() + labs(title="Duration_ms by target")
p4 <- ggplot(data=music,aes(x=energy,fill=target)) + geom_density() + labs(title="Energy by target")
p5 <- ggplot(data=music,aes(x=instrumentalness,fill=target)) + geom_density() + labs(title="Instrumentalness by target")
# include code for plots p2 - p5
plot_grid(p1,p2,p3,p4,p5,ncol=2)

#3 -----------------------------------------------
p1 <- ggplot(data=music,aes(x=liveness,fill=target)) + geom_density() + labs(title="Liveness by target")
p2 <- ggplot(data=music,aes(x=loudness,fill=target)) + geom_density() + labs(title="Loudness by target")
p3 <- ggplot(data=music,aes(x=speechiness,fill=target)) + geom_density() + labs(title="Speechiness by target")
p4 <- ggplot(data=music,aes(x=tempo,fill=target)) + geom_density() + labs(title="Tempo by target")
p5 <- ggplot(data=music,aes(x=valence,fill=target)) + geom_density() + labs(title="Valence by target")
# include code for plots p2 - p5
plot_grid(p1,p2,p3,p4,p5,ncol=2)

#4 -----------------------------------------------
p1 <- ggplot(data=music,aes(x=key)) + geom_bar(aes(fill=target)) + labs(title="Key by target")
p2 <- ggplot(data=music,aes(x=mode)) + geom_bar(aes(fill=target)) + labs(title="Mode by target")
p3 <- ggplot(data=music,aes(x=time_signature)) + geom_bar(aes(fill=target)) + labs(title="Time_signature by target")
plot_grid(p1, p2, p3, ncol=2)

#5 -----------------------------------------------
# Acoustiness, danceability, energy, loudness, tempo, valence. Graphs show some difference between liked and disliked songs.

#6 -----------------------------------------------
idx.train=createDataPartition(y=music$target,p=0.8,list=FALSE)
train=music[idx.train,]
test=music[-idx.train,]
glm.fits=glm(target~.-X-song_title-artist,family=binomial,data=music)
glm.probs=predict(glm.fits,test,type="response")
glm.pred=rep("0",403)
glm.pred[glm.probs >.5]="1"
table(true_classes=test$target,predicted=glm.pred)
mean(glm.pred==test$target)


#7 -----------------------------------------------
idx.train=createDataPartition(y=music$target,p=0.8,list=FALSE)
train=music[idx.train,]
test=music[-idx.train,]
glm.fits=glm(target~.-X-song_title-artist,family=binomial,data=music)
glm.probs=predict(glm.fits,test,type="response")
glm.pred=rep("0",403)
glm.pred[glm.probs >.3]="1"
table(true_classes=test$target,predicted=glm.pred)
mean(glm.pred==test$target)

idx.train=createDataPartition(y=music$target,p=0.8,list=FALSE)
train=music[idx.train,]
test=music[-idx.train,]
glm.fits=glm(target~.-X-song_title-artist,family=binomial,data=music)
glm.probs=predict(glm.fits,test,type="response")
glm.pred=rep("0",403)
glm.pred[glm.probs >.7]="1"
table(true_classes=test$target,predicted=glm.pred)
mean(glm.pred==test$target)


#8 --------------------------------------------------
idx.train=createDataPartition(y=music$target,p=0.8,list=FALSE)
train=music[idx.train,]
test=music[-idx.train,]
glm.fits=glm(target~acousticness+danceability+energy+loudness+tempo+valence,family=binomial,data=music)
glm.probs=predict(glm.fits,test,type="response")
glm.pred=rep("0",403)
glm.pred[glm.probs >.5]="1"
table(true_classes=test$target,predicted=glm.pred)
mean(glm.pred==test$target)


train = lin_reg_train_1
test = lin_reg_test_1
lm.fit=lm(y~X1, data=train)
summary(lm.fit)

lm.fit=lm(y~X1+X3, data=train)
summary(lm.fit)

pred=predict(lm.fit,test)
Rsquared=cor(pred,test$y)^2
Rsquared


lm.fit=lm(y~.+I(X2^2), data=train)
summary(lm.fit)
pred=predict(lm.fit,test)
Rsquared=cor(pred,test$y)^2
Rsquared


train = log_reg_train_1
test = log_reg_test_1
glm.fits=glm(y~.-X1,family=binomial,data=train)
glm.probs=predict(glm.fits,test,type="response")
glm.pred=rep("0",200)
glm.pred[glm.probs >.4]="1"
table(true_classes=test$y,predicted=glm.pred)
mean(glm.pred==test$y)
summary(glm.fits)


train = log_reg_train_1
test = log_reg_test_1
glm.fits=glm(y~.-X3,family=binomial,data=train)
glm.probs=predict(glm.fits,test,type="response")
glm.pred=rep("0",200)
glm.pred[glm.probs >.5]="1"
table(true_classes=test$y,predicted=glm.pred)
mean(glm.pred==test$y)
summary(glm.fits)

train = log_reg_train_1
test = log_reg_test_1
glm.fits=glm(y~.-X1,family=binomial,data=train)
glm.probs=predict(glm.fits,test,type="response")
glm.pred=rep("0",200)
glm.pred[glm.probs >.5]="1"
table(true_classes=test$y,predicted=glm.pred)
mean(glm.pred==test$y)
