import SstNavLink from "./SstNavLink";

export default function Nav() {
  return (
    <nav className="mb-2">
      <ul className="flex flex-col gap-2">
        <SstNavLink to="/transactions">
          <i className="fa-solid fa-fw fa-arrow-right-arrow-left fa-lg"></i>{" "}
          Transactions
        </SstNavLink>
        <SstNavLink to="/categories">
          <i className="fa-solid fa-fw fa-list fa-lg"></i> Categories
        </SstNavLink>
        <SstNavLink to="/cashflow">
          <i className="fa-solid fa-fw fa-dollar-sign fa-lg"></i> Cash Flow
        </SstNavLink>
        <SstNavLink to="/accounts">
          <i className="fa-solid fa-fw fa-building-columns fa-lg"></i> Accounts
        </SstNavLink>
        <SstNavLink to="/import">
          <i className="fa-solid fa-fw fa-download fa-lg"></i> Import
        </SstNavLink>
      </ul>
    </nav>
  );
}
