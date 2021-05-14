rm -rf ./tmp
docker build -t zlib-linux .
id=$(docker create zlib-linux)
docker cp $id:/runtimes ./tmp
docker rm -v $id
rsync -a --remove-source-files tmp/ runtimes/