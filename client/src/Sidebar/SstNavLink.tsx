import { PropsWithChildren } from "react";
import { NavLink } from "react-router-dom";

interface SstNavLinkProps {
    to: string
}

export default function SstNavLink(props: PropsWithChildren<SstNavLinkProps>) {
    function className({isActive, isPending}: {isActive: boolean, isPending: boolean}) {
        return `p-1 py-1.5 rounded transition duration-300 ${isActive ? "bg-gray-600 text-gray-100" : "hover:bg-gray-100"}`;
    }

    return (
        <NavLink to={props.to} className={className}>
            {props.children}
        </NavLink>
    );
}
