select
  pa.providerpickupareaid,
  pa.Day,
  pa.weekrecurrence,
  t.pickupstart,
  lp.name as logisticalprovidername
from 
  providerpickupareaday pa inner join providerpickupareadaytimeslot t on pa.id = t.providerpickupareadayid
  inner join providerpickuparea ppa on ppa.id = pa.providerpickupareaid
  inner join pickuparea par on par.id = ppa.pickupareaid 
  inner join logisticalprovider lp on lp.id = ppa.logisticalproviderid
where 
  par.postalcodefrom <= :postalCode and par.postalcodeto>= :postalCode