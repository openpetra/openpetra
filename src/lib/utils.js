function API_call(url, args, next_function) {

    var r = api.post(url, args).then(
      function (data) {
        data = data.data.d
        data = JSON.parse(data);

        next_function( data );
      }
    );

  return r;

}
