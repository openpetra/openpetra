<?php

    $wsdl = 'https://tracker.openpetra.org/api/soap/mantisconnect.php?wsdl';
    $client = new SoapClient($wsdl, array('trace' => TRUE));


    try
    {
        #$result = $client->mc_projects_get_user_accessible('pokorra', '...' );
        #echo "<pre>".print_r($result, true)."</pre>";
        $result = $client->mc_project_get_versions( 'pokorra', '...',  17 );
        echo "<pre>".print_r($result, true)."</pre>";

         echo '<h2>Request</h2><pre>' . htmlspecialchars($client->__getLastRequest(), ENT_QUOTES) . '</pre>';
        echo '<h2>Response</h2><pre>' . htmlspecialchars($client->__getLastResponse(), ENT_QUOTES) . '</pre>';
        }
    catch(SoapFault $fault){
      // <xmp> tag displays xml output in html
      echo 'Request : <br/><xmp>',
      $client->__getLastRequest(),
      '</xmp><br/><br/> Error Message : <br/>',
      $fault->getMessage();
    }

?>