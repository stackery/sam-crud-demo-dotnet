# Stackery CRUD Demo - C#/.NET

This is a sample template for a serverless AWS Lambda application, written in C#/.NET.

The application implements a CRUD interface in front of an AWS DynamoDB table that
manages a simple user record.  An API Gateway distributes requests to the various
AWS Lambda functions based on their HTTP paths and methods.

The application architecture is defined in template.yaml, a Serverless
Application Model (SAM) template which can be managed through the Stackery UI
at app.stackery.io.

Here is an overview of the files:

```text
.
├── README.md                          <-- This README file
├── src                                <-- Source code dir for all AWS Lambda functions
│   ├── createUser                     <-- Source code dir for createUser function
│   │   ├── .stackery-config.yaml      <-- Default CLI parameters for this directory
│   │   ├── StackeryFunction.csproj    <-- createUser project config
│   │   └── Handler.cs                 <-- Source code for createUser
│   ├── getUser                        <-- Source code dir for getUser function
│   │   ├── .stackery-config.yaml      <-- Default CLI parameters for this directory
│   │   ├── StackeryFunction.csproj    <-- getUser project config
│   │   └── Handler.cs                 <-- Source code for getUser
│   ├── updateUser                     <-- Source code dir for updateUser function
│   │   ├── .stackery-config.yaml      <-- Default CLI parameters for this directory
│   │   ├── StackeryFunction.csproj    <-- updateUser project config
│   │   └── Handler.cs                 <-- Source code for updateUser
│   ├── deleteUser                     <-- Source code dir for deleteUser function
│   │   ├── .stackery-config.yaml      <-- Default CLI parameters for this directory
│   │   ├── StackeryFunction.csproj    <-- deleteUser project config
│   │   └── Handler.cs                 <-- Source code for deleteUser
│   └── listUsers                      <-- Source code dir for listUsers function
│   │   ├── .stackery-config.yaml      <-- Default CLI parameters for this directory
│   │   ├── StackeryFunction.csproj    <-- listUsers project config
│   │   └── Handler.cs                 <-- Source code for listUsers
└── template.yaml                      <-- SAM infrastructure-as-code template
```

One thing to note is that template.yaml was hand-edited to grant each function
dynamodb:DescribeTable access to the table; at the time of this writing, DescribeTable
is not included in DynamoDbCrudPolicy.  See
[this issue report](https://github.com/awslabs/serverless-application-model/issues/509).

Clone this stack in Stackery, deploy it, and test as follows:

- Set `STAGE_LOCATION` from the deployed Rest Api resource properties.

- Create a user:

        curl --header "Content-Type: application/json" \
        --request POST \
        --data '{"id": "unique001", "firstName": "Alice", "lastName": "Smith", "color": "blue"}' \
        ${STAGE_LOCATION}/users

- List users:

        curl ${STAGE_LOCATION}/users

- Read a user:

        curl ${STAGE_LOCATION}/users/unique001

- Update a user:

        curl --header "Content-Type: application/json" \
        --request PUT \
        --data '{"firstName": "Alice", "lastName": "Smith", "color": "green"}' \
        ${STAGE_LOCATION}/users/unique001

- Delete a user:

        curl --request DELETE ${STAGE_LOCATION}/users/unique001
