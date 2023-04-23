import axios from "axios"
import { error } from "console"
import { Button, Header, Segment } from "semantic-ui-react"

export default function TestError() {
  const baseUrl = "http://localhost:5000/api/"

  function handleNotFund() {
    axios.get(baseUrl + "buggy/not-fond").catch((err) => console.log(err.response))
  }

  function handleBadRequest() {
    axios.get(baseUrl + "buggy/bad-request").catch((err) => console.log(err.response))
  }

  function handleServerError() {
    axios.get(baseUrl + "buggy/server-error").catch((err) => console.log(err.response))
  }

  function handleUnauthorised() {
    axios.get(baseUrl + "buggy/unauthorised").catch((err) => console.log(err.response))
  }

  function handleBadGuid() {
    axios.get(baseUrl + "activities/notaguid").catch((err) => console.log(err.response))
  }

  function handleValidationError() {
    axios.get(baseUrl + "activities", {}).catch((err) => console.log(err.response.response))
  }

  return (
    <>
      <Header as="h1" content="Test Error component" />
      <Segment>
        <Button.Group widths="7">
          <Button onClick={handleNotFund} content="Not Found" basic primary />
          <Button onClick={handleBadRequest} content="Bad Request" basic primary />
          <Button onClick={handleValidationError} content="Validation Error" basic primary />
          <Button onClick={handleServerError} content="Server Error" basic primary />
          <Button onClick={handleUnauthorised} content="Unauthorised" basic primary />
          <Button onClick={handleBadGuid} content="Bad Guid" basic primary />
        </Button.Group>
      </Segment>
    </>
  )
}
