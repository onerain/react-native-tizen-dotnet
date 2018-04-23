CXX = g++
CC = gcc
CFLAGS = -Wall -c -I. -fPIC -O2
CXXFLAGS = -Wall -std=c++11 -I. -shared -fPIC -fpermissive -O2

CSOURCES =  $(wildcard *.c)

CXXSOURCES = $(wildcard *.cpp)

OBJSC := $(CSOURCES:.c=.o)
OBJSCXX := $(CXXSOURCES:.cpp=.o)

all: server

server: $(OBJSC) $(OBJSCXX)
	$(CC) -shared -fPIC  $(OBJSC) $(OBJSCXX) -o libyoga.so

$(OBJSC):%.o: %.c
	$(CC) -c -std=c99 -o $@ $< $(CFLAGS)

$(OBJSCXX):%.o: %.cpp
	$(CC) -c -o $@ $< $(CFLAGS)

include:%.o

.PHONY: clean
clean:
	$(RM) ./*.gc??
	$(RM) ./*.o
	$(RM) ./*.so