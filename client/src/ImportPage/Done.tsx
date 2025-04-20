import {useContext} from "react";
import {ImportContext} from "./ImportContext.tsx";
import {Link} from "react-router-dom";

export default function Done() {
    const [context, _] = useContext(ImportContext);

    if (!context.done)
        return;

    return (
        <div className="w-full h-full flex flex-col justify-center items-center">
            <div className="text-green-500 flex flex-col items-center">
                <i className="fa fa-4x fa-check"></i>
                <p className="text-2xl mb-2">Done!</p>
                <Link to="/transactions">
                    <div className="bg-green-100 rounded px-2">
                        <span className="mr-2">Go to transactions</span>
                        <i className="fa fa-arrow-right"></i>
                    </div>
                </Link>
            </div>
        </div>
    );
}