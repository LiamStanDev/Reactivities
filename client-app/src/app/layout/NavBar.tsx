import { NavLink } from "react-router-dom";
import { Button, Container, Menu } from "semantic-ui-react";
/* why don't put NavBar.tsx into feature?
 * Ans: This component has no relation with domain, and it is layout.
 */

function NavBar() {
  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item as={NavLink} to="/" header>
          {" "}
          <img src="/assets/logo.png" alt="logo" style={{ marginRight: "10px" }} />
          Reactivities
        </Menu.Item>
        {/* NavLink會使標籤若在指向的位址會顯示按下，相較於Link更是合作為NavBar使用 */}
        <Menu.Item as={NavLink} to="/activities" name="Activities" />
        <Menu.Item>
          <Button as={NavLink} to="/createActivity" positive content="Create Activity" />
        </Menu.Item>
      </Container>
    </Menu>
  );
}

export default NavBar;
