import { observer } from "mobx-react-lite";
import { Button, Container, Menu } from "semantic-ui-react";
import { useStore } from "../stores/store";
/* why don't put NavBar.tsx into feature?
 * Ans: This component has no relation with domain, and it is layout.
 */

function NavBar() {
  const { activityStore } = useStore();
  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item header>
          <img src="/assets/logo.png" alt="logo" style={{ marginRight: "10px" }} />
          Reactivities
        </Menu.Item>
        <Menu.Item name="Activities" />
        <Menu.Item>
          <Button onClick={() => activityStore.openForm()} positive content="Create Activity" />
        </Menu.Item>
      </Container>
    </Menu>
  );
}

export default observer(NavBar);
