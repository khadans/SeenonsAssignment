select s.stream_product_id, s.type, s.name, ppas.providerpickupareaid as providerpickupareaid 
from  providerpickuparea_stream ppas
inner join stream s on ppas.streamid = s.stream_product_id where ppas.providerpickupareaid = ANY(:ids)