import SstNavLink from "./SstNavLink";

function Nav() {
    return (
        <nav className="mb-2">
            <ul className="flex flex-col gap-2">
                <SstNavLink to="/transactions">
                    <i className="fa-solid fa-fw fa-arrow-right-arrow-left fa-lg"></i> Transactions
                </SstNavLink>
                <SstNavLink to="/targets">
                    <i className="fa-solid fa-fw fa-bullseye fa-lg"></i> Targets
                </SstNavLink>
                <SstNavLink to="/cashflow">
                    <i className="fa-solid fa-fw fa-dollar-sign fa-lg"></i> Cash Flow
                </SstNavLink>
            </ul>
        </nav>
    );
}

export default Nav;
