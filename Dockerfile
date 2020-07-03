FROM ubuntu:16.04 as ubuntu-16
RUN apt-get update && apt-get install -y gcc make
COPY ./zlib-src ./zlib
WORKDIR /zlib
RUN ./configure && make

FROM ubuntu:18.04 as ubuntu-18
RUN apt-get update && apt-get install -y gcc make
COPY ./zlib-src ./zlib
WORKDIR /zlib
RUN ./configure && make

FROM ubuntu:20.04 as ubuntu-20
RUN apt-get update && apt-get install -y gcc make
COPY ./zlib-src ./zlib
WORKDIR /zlib
RUN ./configure && make

FROM debian:9 as debian-9
RUN apt-get update && apt-get install -y gcc make
COPY ./zlib-src ./zlib
WORKDIR /zlib
RUN ./configure && make

FROM debian:10 as debian-10
RUN apt-get update && apt-get install -y gcc make
COPY ./zlib-src ./zlib
WORKDIR /zlib
RUN ./configure && make

FROM centos:7 as centos-7
RUN yum update -y && yum install -y gcc make
COPY ./zlib-src ./zlib
WORKDIR /zlib
RUN ./configure && make

FROM centos:8 as centos-8
RUN yum update -y && yum install -y gcc make
COPY ./zlib-src ./zlib
WORKDIR /zlib
RUN ./configure && make

FROM alpine:latest
RUN mkdir -p ./runtimes/ubuntu.16.04-x64/native
RUN mkdir -p ./runtimes/ubuntu.18.04-x64/native
RUN mkdir -p ./runtimes/ubuntu.20.04-x64/native

RUN mkdir -p ./runtimes/debian.9-x64/native
RUN mkdir -p ./runtimes/debian.10-x64/native

RUN mkdir -p ./runtimes/centos.7-x64/native
RUN mkdir -p ./runtimes/centos.8-x64/native

COPY --from=ubuntu-16 /zlib/libz.so.1 ./runtimes/ubuntu.16.04-x64/native/libz.so
COPY --from=ubuntu-18 /zlib/libz.so.1 ./runtimes/ubuntu.18.04-x64/native/libz.so
COPY --from=ubuntu-20 /zlib/libz.so.1 ./runtimes/ubuntu.20.04-x64/native/libz.so

COPY --from=debian-9 /zlib/libz.so.1 ./runtimes/debian.9-x64/native/libz.so
COPY --from=debian-10 /zlib/libz.so.1 ./runtimes/debian.10-x64/native/libz.so

COPY --from=centos-7 /zlib/libz.so.1 ./runtimes/centos.7-x64/native/libz.so
COPY --from=centos-8 /zlib/libz.so.1 ./runtimes/centos.8-x64/native/libz.so