. /usr/local/petra/petra22env.sh
startdate=01-01-2009
enddate=12-31-2009
specialbirthdays=31,50,60,70,75,80,85,90,95,100,105,110,115
specialanniversary=20,25,30,35,40,45,50,55
output=`mono anniversaries.exe -username:sysadmin_sql -password:petra -fieldkey:27000000 -startdate:$startdate -enddate:$enddate -specialbirthdays:$specialbirthdays -specialanniversary:$specialanniversary -Server.RDBMSType:Progress -Server.ODBC_DSN:petra2_2`
echo "$output" | /usr/sbin/sendmail timotheus.pokorra@d.om.org

