# Alexa.NET.RequestHandlers

## Adding custom Pipelines

private AlexaRequestPipeline<APLSkillRequest> _pipeline = new AlexaRequestPipeline<APLSkillRequest>
(
    new[] {new RollIntent_Speech()}
);