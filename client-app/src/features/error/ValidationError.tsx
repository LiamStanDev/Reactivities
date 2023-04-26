import { Message } from "semantic-ui-react";

interface Props {
  errors: string[];
}

export default function ValidationError({ errors }: Props) {
  return (
    <Message error={true}>
      {errors && (
        <Message.List>
          {errors.map((err, index) => {
            return <Message.Item key={index}>{err}</Message.Item>;
          })}
        </Message.List>
      )}
    </Message>
  );
}
