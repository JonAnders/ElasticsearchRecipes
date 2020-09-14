# ElasticsearchRecipes
An ASP.NET Core project made to try out the Elasticsearch .NET client

It is based on a [dataset of recipes](https://github.com/codingexplained/complete-guide-to-elasticsearch/blob/master/recipes-bulk.json) from the Udemy course [Complete Guide to Elasticsearch](https://www.udemy.com/course/elasticsearch-complete-guide/).

To import the test data, run the following command:
```
$ curl -H "Content-Type: application/x-ndjson" -XPOST http://localhost:9200/recipes/_bulk --data-binary "@recipes-bulk.json"
```
