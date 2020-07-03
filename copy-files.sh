rm -rf ./tmp
id=$(docker create zlib-linux)
docker cp $id:/runtimes ./tmp
docker rm -v $id
rsync -a --remove-source-files tmp/ runtimes/