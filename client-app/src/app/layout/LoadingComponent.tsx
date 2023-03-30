import { Dimmer, Loader } from "semantic-ui-react";

interface Props {
  inverted?: boolean;
  content?: string;
}

// inverted is false: the Dimmer will let the screen dark.
export default function LoadingComponent({
  inverted = true,
  content = "Loading...",
}: Props) {
  // active = true: the dimmer gonna show the Loading circle.
  return (
    <Dimmer active={true} inverted={inverted}>
      <Loader content={content} />
    </Dimmer>
  );
}
