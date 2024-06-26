import { useEffect, useState } from 'react';
import { MsalAuthenticationTemplate } from '@azure/msal-react';
import { InteractionType } from '@azure/msal-browser';

import { ListView } from '../components/ListView';
import { loginRequest, protectedResources } from "../authConfig";
import useFetchWithMsal from '../hooks/useFetchWithMsal';

const WeatherForcastListContent = () => {
    const { error, execute } = useFetchWithMsal({
        scopes: protectedResources.apiWeatherForcast.scopes.read,
    });

    const [weatherForcastListData, setWeatherForcastListData] = useState(null);
    
    useEffect(() => {
        if (!weatherForcastListData) {
            execute("GET", "https://localhost:7231/WeatherForecast").then((response) => {
                setWeatherForcastListData(response);
            });
        }
    }, [execute, weatherForcastListData])

    if (error) {
        return <div>Error: {error.message}</div>;
    }

    return <>{weatherForcastListData ? <p>{weatherForcastListData}</p> : null}</>;
};

/**
 * The `MsalAuthenticationTemplate` component will render its children if a user is authenticated
 * or attempt to sign a user in. Just provide it with the interaction type you would like to use
 * (redirect or popup) and optionally a request object to be passed to the login API, a component to display while
 * authentication is in progress or a component to display if an error occurs. For more, visit:
 * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-react/docs/getting-started.md
 */
export const WeatherForcastList = () => {
    const authRequest = {
        ...loginRequest,
    };

    return (
        <MsalAuthenticationTemplate 
            interactionType={InteractionType.Redirect} 
            authenticationRequest={authRequest}
        >
            <WeatherForcastListContent />
        </MsalAuthenticationTemplate>
    );
};
